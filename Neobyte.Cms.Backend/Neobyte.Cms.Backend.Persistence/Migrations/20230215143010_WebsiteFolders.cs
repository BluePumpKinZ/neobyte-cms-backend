using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WebsiteFolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_HostingConnections_ConnectionId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_ConnectionId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Templates");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Templates",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "HomeFolder",
                table: "Websites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UploadFolder",
                table: "Websites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeFolder",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "UploadFolder",
                table: "Websites");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Templates",
                newName: "Name");

            migrationBuilder.AddColumn<Guid>(
                name: "ConnectionId",
                table: "Templates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_ConnectionId",
                table: "Templates",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_HostingConnections_ConnectionId",
                table: "Templates",
                column: "ConnectionId",
                principalTable: "HostingConnections",
                principalColumn: "Id");
        }
    }
}
