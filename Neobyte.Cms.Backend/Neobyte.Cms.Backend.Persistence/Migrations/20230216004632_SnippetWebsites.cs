using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SnippetWebsites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snippets_Websites_WebsiteEntityId",
                table: "Snippets");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Snippets");

            migrationBuilder.RenameColumn(
                name: "WebsiteEntityId",
                table: "Snippets",
                newName: "WebsiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Snippets_WebsiteEntityId",
                table: "Snippets",
                newName: "IX_Snippets_WebsiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snippets_Websites_WebsiteId",
                table: "Snippets",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snippets_Websites_WebsiteId",
                table: "Snippets");

            migrationBuilder.RenameColumn(
                name: "WebsiteId",
                table: "Snippets",
                newName: "WebsiteEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Snippets_WebsiteId",
                table: "Snippets",
                newName: "IX_Snippets_WebsiteEntityId");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Snippets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Snippets_Websites_WebsiteEntityId",
                table: "Snippets",
                column: "WebsiteEntityId",
                principalTable: "Websites",
                principalColumn: "Id");
        }
    }
}
