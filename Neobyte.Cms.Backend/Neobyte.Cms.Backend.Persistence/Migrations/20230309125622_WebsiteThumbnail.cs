using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WebsiteThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Websites",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Websites");
        }
    }
}
