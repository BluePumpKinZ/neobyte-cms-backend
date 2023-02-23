using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neobyte.Cms.Backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WebsiteAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteAccounts_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAccounts_AccountId",
                table: "WebsiteAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteAccounts_WebsiteId",
                table: "WebsiteAccounts",
                column: "WebsiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteAccounts");
        }
    }
}
