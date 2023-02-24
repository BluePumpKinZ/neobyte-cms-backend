using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SftpConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SftpHostingConnectionEntity_Host",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SftpHostingConnectionEntity_Password",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SftpHostingConnectionEntity_Port",
                table: "HostingConnections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SftpHostingConnectionEntity_Username",
                table: "HostingConnections",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SftpHostingConnectionEntity_Host",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "SftpHostingConnectionEntity_Password",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "SftpHostingConnectionEntity_Port",
                table: "HostingConnections");

            migrationBuilder.DropColumn(
                name: "SftpHostingConnectionEntity_Username",
                table: "HostingConnections");
        }
    }
}
