﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Neobyte.Cms.Backend.Persistence.EF;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    [DbContext(typeof(EFDbContext))]
    partial class EFDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Accounts.AccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.HostingConnectionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HostingConnections");

                    b.HasDiscriminator<string>("Discriminator").HasValue("HostingConnectionEntity");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.HtmlContentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Html")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HtmlContents");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.PageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TemplateEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TemplateEntityId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.SnippetEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WebsiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.HasIndex("TemplateId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Snippets");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.TemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid?>("HtmlContentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("HtmlContentId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteAccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("WebsiteAccounts");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ConnectionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HomeFolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("UploadFolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId");

                    b.ToTable("Websites");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.FtpHostingConnectionEntity", b =>
                {
                    b.HasBaseType("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.HostingConnectionEntity");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("HostingConnections");

                    b.HasDiscriminator().HasValue("FtpHostingConnectionEntity");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.SftpHostingConnectionEntity", b =>
                {
                    b.HasBaseType("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.HostingConnectionEntity");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("HostingConnections", t =>
                        {
                            t.Property("Host")
                                .HasColumnName("SftpHostingConnectionEntity_Host");

                            t.Property("Password")
                                .HasColumnName("SftpHostingConnectionEntity_Password");

                            t.Property("Port")
                                .HasColumnName("SftpHostingConnectionEntity_Port");

                            t.Property("Username")
                                .HasColumnName("SftpHostingConnectionEntity_Username");
                        });

                    b.HasDiscriminator().HasValue("SftpHostingConnectionEntity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Accounts.IdentityAccountEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.AccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.PageEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.TemplateEntity", null)
                        .WithMany("Pages")
                        .HasForeignKey("TemplateEntityId");

                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", "Website")
                        .WithMany("Pages")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Website");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.SnippetEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.HtmlContentEntity", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.TemplateEntity", "Template")
                        .WithMany("Snippets")
                        .HasForeignKey("TemplateId");

                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", "Website")
                        .WithMany("Snippets")
                        .HasForeignKey("WebsiteId");

                    b.Navigation("Content");

                    b.Navigation("Template");

                    b.Navigation("Website");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.TemplateEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.HtmlContentEntity", "HtmlContent")
                        .WithMany()
                        .HasForeignKey("HtmlContentId");

                    b.Navigation("HtmlContent");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteAccountEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Accounts.AccountEntity", "Account")
                        .WithMany("WebsiteAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", "Website")
                        .WithMany("WebsiteAccounts")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Website");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", b =>
                {
                    b.HasOne("Neobyte.Cms.Backend.Persistence.Entities.Websites.HostingConnections.HostingConnectionEntity", "Connection")
                        .WithMany()
                        .HasForeignKey("ConnectionId");

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Accounts.AccountEntity", b =>
                {
                    b.Navigation("WebsiteAccounts");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.TemplateEntity", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("Snippets");
                });

            modelBuilder.Entity("Neobyte.Cms.Backend.Persistence.Entities.Websites.WebsiteEntity", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("Snippets");

                    b.Navigation("WebsiteAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
