using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CandidateTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BgVerifiedById",
                table: "Candidate",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Candidate",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "Candidate",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Candidate",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Candidate",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBgVerificationCompleted",
                table: "Candidate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Candidate",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Candidate",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Candidate",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CandidateSkill",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSkill", x => new { x.CandidateId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_CandidateSkill_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    VerifiedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateDocument_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateDocument_DocumentType_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_BgVerifiedById",
                table: "Candidate",
                column: "BgVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDocument_CandidateId",
                table: "CandidateDocument",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateDocument_DocumentTypeId",
                table: "CandidateDocument",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkill_SkillId",
                table: "CandidateSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_Name",
                table: "DocumentType",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidate_User_BgVerifiedById",
                table: "Candidate",
                column: "BgVerifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidate_User_BgVerifiedById",
                table: "Candidate");

            migrationBuilder.DropTable(
                name: "CandidateDocument");

            migrationBuilder.DropTable(
                name: "CandidateSkill");

            migrationBuilder.DropTable(
                name: "DocumentType");

            migrationBuilder.DropIndex(
                name: "IX_Candidate_BgVerifiedById",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "BgVerifiedById",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "IsBgVerificationCompleted",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Candidate");
        }
    }
}