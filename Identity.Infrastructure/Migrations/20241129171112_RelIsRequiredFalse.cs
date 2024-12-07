using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelIsRequiredFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JwtTokens_Accounts_AccountId",
                table: "JwtTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_JwtTokens_Accounts_AccountId",
                table: "JwtTokens",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JwtTokens_Accounts_AccountId",
                table: "JwtTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_JwtTokens_Accounts_AccountId",
                table: "JwtTokens",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
