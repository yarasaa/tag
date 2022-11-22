﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using amorphie.tag.data;

#nullable disable

namespace amorphie.tag.data.Migrations
{
    [DbContext(typeof(TagDBContext))]
    partial class TagDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("amorphie.tag.data.Domain", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Domains");

                    b.HasData(
                        new
                        {
                            Name = "idm",
                            Description = "Identity Management Platform"
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.Entity", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DomainName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.HasIndex("DomainName");

                    b.ToTable("Entites");

                    b.HasData(
                        new
                        {
                            Name = "user",
                            Description = "User repository",
                            DomainName = "idm"
                        },
                        new
                        {
                            Name = "scope",
                            Description = "Scope repository",
                            DomainName = "idm"
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.EntityData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EntityName")
                        .HasColumnType("text");

                    b.Property<string>("Field")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Ttl")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EntityName");

                    b.ToTable("EntityData");

                    b.HasData(
                        new
                        {
                            Id = new Guid("107f4644-57cd-46ff-80de-004c6cd44704"),
                            EntityName = "user",
                            Field = "firstname"
                        },
                        new
                        {
                            Id = new Guid("207f4644-57cd-46ff-80de-004c6cd44704"),
                            EntityName = "user",
                            Field = "lastname"
                        },
                        new
                        {
                            Id = new Guid("307f4644-57cd-46ff-80de-004c6cd44704"),
                            EntityName = "scope",
                            Field = "title"
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.EntityDataSource", b =>
                {
                    b.Property<Guid>("EntityDataId")
                        .HasColumnType("uuid");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<string>("DataPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.HasKey("EntityDataId", "Order");

                    b.HasIndex("TagName");

                    b.ToTable("EntityDataSource");

                    b.HasData(
                        new
                        {
                            EntityDataId = new Guid("107f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 1,
                            DataPath = "$.firstname",
                            TagName = "burgan-staff"
                        },
                        new
                        {
                            EntityDataId = new Guid("107f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 2,
                            DataPath = "$.partner-staff.fullname",
                            TagName = "loan-partner-staff"
                        },
                        new
                        {
                            EntityDataId = new Guid("107f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 3,
                            DataPath = "$.firstname",
                            TagName = "retail-customer"
                        },
                        new
                        {
                            EntityDataId = new Guid("207f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 1,
                            DataPath = "$.lastname",
                            TagName = "burgan-staff"
                        },
                        new
                        {
                            EntityDataId = new Guid("207f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 2,
                            DataPath = "$.partner-staff.fullname",
                            TagName = "loan-partner-staff"
                        },
                        new
                        {
                            EntityDataId = new Guid("207f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 3,
                            DataPath = "$.lastname",
                            TagName = "retail-customer"
                        },
                        new
                        {
                            EntityDataId = new Guid("307f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 1,
                            DataPath = "$.firstname",
                            TagName = "burgan-staff"
                        },
                        new
                        {
                            EntityDataId = new Guid("307f4644-57cd-46ff-80de-004c6cd44704"),
                            Order = 2,
                            DataPath = "$.partner-staff.fullname",
                            TagName = "loan-partner-staff"
                        });
                });

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
                            Ttl = 5,
                            Url = "http://localhost:3000/cb.customers?reference=@reference"
                        },
                        new
                        {
                            Name = "corporate-customer",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.customers?reference=@reference"
                        },
                        new
                        {
                            Name = "loan-partner",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.partner/@reference"
                        },
                        new
                        {
                            Name = "loan-partner-staff",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.partner/@partner/staff/@reference"
                        },
                        new
                        {
                            Name = "burgan-staff",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.staff/@reference"
                        },
                        new
                        {
                            Name = "burgan-bank-turkey",
                            Ttl = 10,
                            Url = "http://localhost:3000/cb.bankInfo"
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

                    b.HasData(
                        new
                        {
                            OwnerName = "idm",
                            TagName = "corporate-customer"
                        },
                        new
                        {
                            OwnerName = "idm",
                            TagName = "retail-customer"
                        },
                        new
                        {
                            OwnerName = "idm",
                            TagName = "loan-partner"
                        },
                        new
                        {
                            OwnerName = "idm",
                            TagName = "loan-partner-staff"
                        },
                        new
                        {
                            OwnerName = "idm",
                            TagName = "burgan-staff"
                        },
                        new
                        {
                            OwnerName = "idm",
                            TagName = "burgan-bank-turkey"
                        },
                        new
                        {
                            OwnerName = "retail-loan",
                            TagName = "retail-customer"
                        },
                        new
                        {
                            OwnerName = "retail-loan",
                            TagName = "loan-partner"
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.View", b =>
                {
                    b.Property<string>("TagName")
                        .HasColumnType("text");

                    b.Property<string>("ViewTemplateName")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("TagName", "ViewTemplateName");

                    b.ToTable("Views");

                    b.HasData(
                        new
                        {
                            TagName = "retail-customer",
                            ViewTemplateName = "retail-customer-mini-html",
                            Type = 0
                        },
                        new
                        {
                            TagName = "retail-customer",
                            ViewTemplateName = "retail-customer-flutter",
                            Type = 2
                        });
                });

            modelBuilder.Entity("amorphie.tag.data.Entity", b =>
                {
                    b.HasOne("amorphie.tag.data.Domain", "Domain")
                        .WithMany("Entities")
                        .HasForeignKey("DomainName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Domain");
                });

            modelBuilder.Entity("amorphie.tag.data.EntityData", b =>
                {
                    b.HasOne("amorphie.tag.data.Entity", "Entity")
                        .WithMany("Data")
                        .HasForeignKey("EntityName");

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("amorphie.tag.data.EntityDataSource", b =>
                {
                    b.HasOne("amorphie.tag.data.EntityData", "EntityData")
                        .WithMany("Sources")
                        .HasForeignKey("EntityDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("amorphie.tag.data.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagName");

                    b.Navigation("EntityData");

                    b.Navigation("Tag");
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

            modelBuilder.Entity("amorphie.tag.data.View", b =>
                {
                    b.HasOne("amorphie.tag.data.Tag", "Tag")
                        .WithMany("Views")
                        .HasForeignKey("TagName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("amorphie.tag.data.Domain", b =>
                {
                    b.Navigation("Entities");
                });

            modelBuilder.Entity("amorphie.tag.data.Entity", b =>
                {
                    b.Navigation("Data");
                });

            modelBuilder.Entity("amorphie.tag.data.EntityData", b =>
                {
                    b.Navigation("Sources");
                });

            modelBuilder.Entity("amorphie.tag.data.Tag", b =>
                {
                    b.Navigation("Tags");

                    b.Navigation("Views");
                });
#pragma warning restore 612, 618
        }
    }
}
