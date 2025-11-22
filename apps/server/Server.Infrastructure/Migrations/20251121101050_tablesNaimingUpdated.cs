using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tablesNaimingUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionBatch_Designation_DesignationId",
                table: "PositionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionBatchReviewer_User_ReviewerUserId",
                table: "PositionBatchReviewer");

            migrationBuilder.DropTable(
                name: "JobOpeningInterviewPanelRequirement");

            migrationBuilder.DropTable(
                name: "JobOpeningInterviewRoundTemplate");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Designation");

            migrationBuilder.DropColumn(
                name: "GoogleId",
                table: "Auth");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Role",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Role_RoleName",
                table: "Role",
                newName: "IX_Role_Name");

            migrationBuilder.RenameColumn(
                name: "ReviewerUserId",
                table: "PositionBatchReviewer",
                newName: "ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_PositionBatchReviewer_ReviewerUserId",
                table: "PositionBatchReviewer",
                newName: "IX_PositionBatchReviewer_ReviewerId");

            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "Interview",
                newName: "DurationInMinutes");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DesignationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    ContactNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterviewRoundTemplate",
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
                    table.PrimaryKey("PK_InterviewRoundTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewRoundTemplate_JobOpening_JobOpeningId",
                        column: x => x.JobOpeningId,
                        principalTable: "JobOpening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewPanelRequirement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    RequiredCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewPanelRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewPanelRequirement_InterviewRoundTemplate_InterviewT~",
                        column: x => x.InterviewTemplateId,
                        principalTable: "InterviewRoundTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_AssignedBy",
                table: "UserRole",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DesignationId",
                table: "Employee",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewPanelRequirement_InterviewTemplateId_Role",
                table: "InterviewPanelRequirement",
                columns: new[] { "InterviewTemplateId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRoundTemplate_JobOpeningId_RoundNumber",
                table: "InterviewRoundTemplate",
                columns: new[] { "JobOpeningId", "RoundNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionBatch_Designation_DesignationId",
                table: "PositionBatch",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionBatchReviewer_User_ReviewerId",
                table: "PositionBatchReviewer",
                column: "ReviewerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_AssignedBy",
                table: "UserRole",
                column: "AssignedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionBatch_Designation_DesignationId",
                table: "PositionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionBatchReviewer_User_ReviewerId",
                table: "PositionBatchReviewer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_AssignedBy",
                table: "UserRole");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "InterviewPanelRequirement");

            migrationBuilder.DropTable(
                name: "InterviewRoundTemplate");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_AssignedBy",
                table: "UserRole");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Role",
                newName: "RoleName");

            migrationBuilder.RenameIndex(
                name: "IX_Role_Name",
                table: "Role",
                newName: "IX_Role_RoleName");

            migrationBuilder.RenameColumn(
                name: "ReviewerId",
                table: "PositionBatchReviewer",
                newName: "ReviewerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PositionBatchReviewer_ReviewerId",
                table: "PositionBatchReviewer",
                newName: "IX_PositionBatchReviewer_ReviewerUserId");

            migrationBuilder.RenameColumn(
                name: "DurationInMinutes",
                table: "Interview",
                newName: "DurationMinutes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Skill",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Designation",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GoogleId",
                table: "Auth",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobOpeningInterviewRoundTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobOpeningId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DurationInMinutes = table.Column<int>(type: "integer", nullable: false),
                    RoundNumber = table.Column<int>(type: "integer", nullable: false),
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
                    RequiredCount = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
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
                name: "FK_PositionBatch_Designation_DesignationId",
                table: "PositionBatch",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionBatchReviewer_User_ReviewerUserId",
                table: "PositionBatchReviewer",
                column: "ReviewerUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}