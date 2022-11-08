var client = new DaprClientBuilder().Build();

var builder = WebApplication.CreateBuilder(args);

var STATE_STORE = "tag-execute-cache";

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCloudEvents();
app.UseRouting();
app.MapSubscribeHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/tag/{tag-name}/execute", ExecuteTag)
.WithOpenApi(operation =>
{
    operation.Summary = "Executes given tag with using body parameters.";
    return operation;
})
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.Produces(StatusCodes.Status510NotExtended)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status204NoContent);


try
{
    app.Logger.LogInformation("Starting application...");
    app.Run();

}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Aplication is terminated unexpectedly ");
}

async Task<IResult> ExecuteTag(
    [FromRoute(Name = "tag-name")] string tagName,
    HttpRequest request,
    HttpContext httpContext
    )
{
    app.Logger.LogInformation("ExecuteTag is calling");

    GetTagResponse? tag;

    try
    {
        tag = await client.InvokeMethodAsync<GetTagResponse>(HttpMethod.Get, "amorphie-tag", "tag/" + tagName);
    }
    catch (Dapr.Client.InvocationException ex)
    {
        if (ex.Response.StatusCode == HttpStatusCode.NotFound)
            return Results.NotFound("Tag is not found.");

        if (ex.Response.StatusCode == HttpStatusCode.InternalServerError)
            return Results.Problem("Tag query service is unavailable", null, 510);

        return Results.Problem($"Tag query service error : {ex.Response.StatusCode}", null, 510);
    }
    catch (Exception ex)
    {

        return Results.Problem($"Unhandled Tag query service error : {ex.Message}", null, 510);
    }

    if (string.IsNullOrEmpty(tag.Url))
    {
        return Results.BadRequest("This tag does not have URL");
    }

    var parameters = tag.Url.Split(new Char[] { '/', '?', '&', '=' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith('@')).ToList();
    var urlToConsume = tag.Url;

    foreach (var p in parameters)
    {
        if (!request.Query.ContainsKey(p.TrimStart('@')))
            return Results.BadRequest($"Required Url parameter(s) is not supplied as query parameters. Required parameters : {string.Join(",", parameters)}");

        urlToConsume = urlToConsume.Replace(p, request.Query[p.TrimStart('@')].ToString());
    }


    var cachedResponse = await client.GetStateAsync<dynamic>(STATE_STORE, urlToConsume);

    if (cachedResponse is not null)
    {
        httpContext.Response.Headers.Add("X-Content-Source", "Cache");
        return Results.Ok(cachedResponse);
    }
    else
    {
        // This process will be replaced with with dapr 1.10 version service invoke for better telemetry: https://github.com/dapr/dapr/issues/4549
        HttpClient httpClient = new();
        var response = await httpClient.GetFromJsonAsync<dynamic>(urlToConsume);

        var metadata = new Dictionary<string, string> { { "ttlInSeconds", $"{tag.Ttl}" } };
        await client.SaveStateAsync(STATE_STORE, urlToConsume, response, metadata: metadata);

        httpContext.Response.Headers.Add("X-Content-Source", "Original");
        return Results.Ok(response);
    }
};






