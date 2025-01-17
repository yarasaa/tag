
public static class TagModule
{
    public static void MapTagEndpoints(this WebApplication app)
    {
        app.MapGet("/tag", getAllTags)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Returns saved tag records.";
            operation.Parameters[0].Description = "Filtering parameter. Given **tag** is used to filter tags.";
            operation.Parameters[1].Description = "Paging parameter. **limit** is the page size of resultset.";
            operation.Parameters[2].Description = "Paging parameter. **Token** is returned from last query.";
            return operation;
        })
        .Produces<GetTagResponse[]>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent);

        app.MapGet("/tag/{tag-name}", getTag)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Returns requested tag.";
            operation.Parameters[0].Description = "Name of the requested tag.";
            return operation;
        })
        .Produces<GetTagResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/tag/{tag-name}/tags", addTagToTag)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Add tag to tag :)";
            operation.Parameters[0].Description = "Tag name";
            return operation;
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status304NotModified)
        .Produces(StatusCodes.Status404NotFound);

        app.MapDelete("/tag/{tag-name}/tags/{tag-name-to-delete}", deleteTagFromTag)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete tag from tag ";
            operation.Parameters[0].Description = "Tag name";
            operation.Parameters[1].Description = "Tag name to be deleted.";
            return operation;
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status304NotModified)
        .Produces(StatusCodes.Status404NotFound);


        app.MapPost("/tag", saveTag)
        .WithTopic("pubsub", "SaveTag") // Default topic for bulk save requirement
        .WithOpenApi(operation =>
        {
            operation.Summary = "Saves or updates requested tag.";
            return operation;
        })
        .Produces<GetTagResponse>(StatusCodes.Status200OK);

        app.MapDelete("/tag/{tag-name}", deleteTag)
        .WithOpenApi(operation =>
        {
            operation.Summary = "Deletes existing tag.";
            return operation;
        })
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status200OK);


    }

    static IResult getAllTags(
        [FromServices] TagDBContext context,
        [FromQuery] string? tagName,
        [FromQuery][Range(0, 100)] int page = 0,
        [FromQuery][Range(5, 100)] int pageSize = 100
        )
    {
        var query = context!.Tags!
            .Include(t => t.Tags)
            .Skip(page * pageSize)
            .Take(pageSize);

        if (!string.IsNullOrEmpty(tagName))
        {
            query.Where(t => t.Tags.Any(c => c.TagName == tagName));
        }

        var tags = query.ToList();

        if (tags.Count() > 0)
        {
            return Results.Ok(tags.Select(tag =>
              new GetTagResponse(
                tag.Name,
                tag.Url,
                tag.Ttl,
                tag.Tags.Select(i => i.TagName).ToArray()
                )
            ).ToArray());
        }
        else
            return Results.NoContent();
    }

    static IResult getTag(
        [FromRoute(Name = "tag-name")] string tagName,
        [FromServices] TagDBContext context
        )
    {

        var tag = context!.Tags!
            .Include(st => st.Tags)
            .Where(t => t.Name == tagName)
            .FirstOrDefault();

        if (tag == null)
            return Results.NotFound();

        return Results.Ok(
            new GetTagResponse(tag.Name, tag.Url, tag.Ttl, tag.Tags.Select(i => i.TagName).ToArray())
        );
    }

    static IResult saveTag(
        [FromBody] SaveTagRequest data,
        [FromServices] TagDBContext context
        )
    {

        var existingRecord = context?.Tags?.FirstOrDefault(t => t.Name == data.Name);

        if (existingRecord == null)
        {
            context!.Tags!.Add(new Tag { Name = data.Name, Url = data.Url, Ttl = data.Ttl ?? 0 });
            context.SaveChanges();
            return Results.Created($"/tag/{data.Name}", existingRecord);
        }
        else
        {
            var hasChanges = false;
            // Apply update to only changed fields.
            if (data.Url != null && data.Url != existingRecord.Url) { existingRecord.Url = data.Url; hasChanges = true; }
            if (data.Ttl != null && data.Ttl != existingRecord.Ttl) { existingRecord.Ttl = data.Ttl.Value; hasChanges = true; }

            if (hasChanges)
            {
                context!.SaveChanges();
                return Results.Ok();
            }
            else
            {
                return Results.Problem("Not Modified.", null, 304);
            }
        }
    }

    static IResult deleteTag(
        [FromRoute(Name = "tag-name")] string tagName,
        [FromServices] TagDBContext context)
    {

        var existingRecord = context?.Tags?.FirstOrDefault(t => t.Name == tagName);

        if (existingRecord == null)
        {
            return Results.NotFound();
        }
        else
        {
            context!.Remove(existingRecord);
            context.SaveChanges();
            return Results.Ok();
        }
    }

    static IResult addTagToTag(
        [FromRoute(Name = "tag-name")] string tagName,
        [FromBody] string tagNameToAdd,
        [FromServices] TagDBContext context
        )
    {
        var tag = context!.Tags!
            .Include(t => t.Tags)
            .Where(t => t.Name == tagName)
            .FirstOrDefault();

        if (tag == null)
            return Results.NotFound("Tag is not found");

        var tagToAdd = context!.Tags!.FirstOrDefault(t => t.Name == tagNameToAdd);

        if (tagToAdd == null)
            return Results.NotFound("Tag to add is not found");

        if (tag.Tags.Any(t => t.TagName == tagNameToAdd))
            return Results.Problem("Not Modified. Tag already assigned.", null, 304);

        tag.Tags.Add(new TagRelation { TagName = tagNameToAdd, OwnerName = tagName });
        context.SaveChanges();
        return Results.Ok();

    }

    static IResult deleteTagFromTag(
        [FromRoute(Name = "tag-name")] string tagName,
        [FromRoute(Name = "tag-name-to-delete")] string tagNameToDelete,
        [FromServices] TagDBContext context
        )
    {
        var tag = context!.Tags!
            .Include(t => t.Tags)
            .Where(t => t.Name == tagName)
            .FirstOrDefault();

        if (tag == null)
            return Results.NotFound("Tag is not found");

        var tagToDelete = context!.TagRelations!.FirstOrDefault(t => t.TagName == tagNameToDelete);


        if (tagToDelete == null)
            return Results.NotFound("Tag to delete is not found");

        context.Remove(tagToDelete);
        context.SaveChanges();
        return Results.Ok();

    }

}