using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class S3Connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessKey",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BucketName",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessKey",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "BucketName",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "HostingConnections");
        }
    }
}
