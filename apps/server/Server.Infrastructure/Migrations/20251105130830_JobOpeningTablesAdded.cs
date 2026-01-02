using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JobOpeningTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PositionBatchId",
                table: "SkillOverRide",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "JobOpeningId",
                table: "SkillOverRide",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobOpening",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    PositionBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpening_PositionBatch_PositionBatchId",
                        column: x => x.PositionBatchId,
                        principalTable: "PositionBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOpening_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOpening_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOpening_User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobOpeningInterviewer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOpeningId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpeningInterviewer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpeningInterviewer_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOpeningInterviewer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobOpeningInterviewRoundTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOpeningId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    RoundNumber = table.Column<int>(type: "integer", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpeningInterviewRoundTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpeningInterviewRoundTemplate_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOpeningInterviewPanelRequirement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOpeningInterviewTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    RequiredCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOpeningInterviewPanelRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOpeningInterviewPanelRequirement_JobOpeningInterviewRoun~",
                        column: x => x.JobOpeningInterviewTemplateId,
                        principalTable: "JobOpeningInterviewRoundTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillOverRide_JobOpeningId",
                table: "SkillOverRide",
                column: "JobOpeningId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_CreatedBy",
                table: "JobOpening",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_DeletedBy",
                table: "JobOpening",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_LastUpdatedBy",
                table: "JobOpening",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpening_PositionBatchId",
                table: "JobOpening",
                column: "PositionBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpeningInterviewer_JobOpeningId_UserId_Role",
                table: "JobOpeningInterviewer",
                columns: new[] { "JobOpeningId", "UserId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOpeningInterviewer_UserId",
                table: "JobOpeningInterviewer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpeningInterviewPanelRequirement_JobOpeningInterviewTemp~",
                table: "JobOpeningInterviewPanelRequirement",
                columns: new[] { "JobOpeningInterviewTemplateId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOpeningInterviewRoundTemplate_JobOpeningId_RoundNumber",
                table: "JobOpeningInterviewRoundTemplate",
                columns: new[] { "JobOpeningId", "RoundNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillOverRide_JobOpening_JobOpeningId",
                table: "SkillOverRide",
                column: "JobOpeningId",
                principalTable: "JobOpening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillOverRide_JobOpening_JobOpeningId",
                table: "SkillOverRide");

            migrationBuilder.DropTable(
                name: "JobOpeningInterviewer");

            migrationBuilder.DropTable(
                name: "JobOpeningInterviewPanelRequirement");

            migrationBuilder.DropTable(
                name: "JobOpeningInterviewRoundTemplate");

            migrationBuilder.DropTable(
                name: "JobOpening");

            migrationBuilder.DropIndex(
                name: "IX_SkillOverRide_JobOpeningId",
                table: "SkillOverRide");

            migrationBuilder.DropColumn(
                name: "JobOpeningId",
                table: "SkillOverRide");

            migrationBuilder.AlterColumn<Guid>(
                name: "PositionBatchId",
                table: "SkillOverRide",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}