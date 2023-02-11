using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DomainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Accounts",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Accounts",
                newName: "Bio");

            migrationBuilder.CreateTable(
                name: "HostingConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostingConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HtmlContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Html = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Websites_HostingConnections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "HostingConnections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HtmlContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_HostingConnections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "HostingConnections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Templates_HtmlContents_HtmlContentId",
                        column: x => x.HtmlContentId,
                        principalTable: "HtmlContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Templates_TemplateEntityId",
                        column: x => x.TemplateEntityId,
                        principalTable: "Templates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pages_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Snippets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebsiteEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snippets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Snippets_HtmlContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "HtmlContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Snippets_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Snippets_Websites_WebsiteEntityId",
                        column: x => x.WebsiteEntityId,
                        principalTable: "Websites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TemplateEntityId",
                table: "Pages",
                column: "TemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_WebsiteId",
                table: "Pages",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_ContentId",
                table: "Snippets",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_TemplateId",
                table: "Snippets",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_WebsiteEntityId",
                table: "Snippets",
                column: "WebsiteEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_ConnectionId",
                table: "Templates",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_HtmlContentId",
                table: "Templates",
                column: "HtmlContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Websites_ConnectionId",
                table: "Websites",
                column: "ConnectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Snippets");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Websites");

            migrationBuilder.DropTable(
                name: "HtmlContents");

            migrationBuilder.DropTable(
                name: "HostingConnections");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Accounts",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Accounts",
                newName: "Firstname");
        }
    }
}
