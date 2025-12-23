using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CandidateUpdated_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerifiedBy",
                table: "CandidateDocument",
                newName: "VerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDocument_VerifiedById",
                table: "CandidateDocument",
                column: "VerifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateDocument_User_VerifiedById",
                table: "CandidateDocument",
                column: "VerifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateDocument_User_VerifiedById",
                table: "CandidateDocument");

            migrationBuilder.DropIndex(
                name: "IX_CandidateDocument_VerifiedById",
                table: "CandidateDocument");

            migrationBuilder.RenameColumn(
                name: "VerifiedById",
                table: "CandidateDocument",
                newName: "VerifiedBy");
        }
    }
}