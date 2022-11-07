﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using amorphie.tag.data;

#nullable disable

namespace amorphie.tag.data.Migrations
{
    [DbContext(typeof(TagDBContext))]
    [Migration("20221107095150_seed2")]
    partial class seed2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("amorphie.tag.data.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("Ttl")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Name = "retail-loan"
                        },
                        new
                        {
                            Name = "idm"
                        },
                        new
                        {
                            Name = "retail-customer",
                            Ttl = 100,
                            Url = "http://localhost:3000/cb.customers/@param1?dbid=@param2&loveid=@param3"
                        },
                        new
                        {
                            Name = "corporate-customer",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.customers/@param1"
                        },
                        new
                        {
                            Name = "loan-partner",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.partner/@partner"
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.TagRelation", b =>
                {
                    b.Property<string>("OwnerName")
                        .HasColumnType("text");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.HasKey("OwnerName", "TagName");

                    b.HasIndex("TagName");

                    b.ToTable("TagRelations");
                });

            modelBuilder.Entity("amorphie.tag.data.TagRelation", b =>
                {
                    b.HasOne("amorphie.tag.data.Tag", "Owner")
                        .WithMany("Tags")
                        .HasForeignKey("OwnerName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("amorphie.tag.data.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("amorphie.tag.data.Tag", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
