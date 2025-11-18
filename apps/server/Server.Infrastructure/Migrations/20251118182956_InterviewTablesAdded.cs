using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InterviewTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InterviewId",
                table: "Feedback",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Interview",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoundNumber = table.Column<int>(type: "integer", nullable: false),
                    InterviewType = table.Column<int>(type: "integer", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    MeetingLink = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interview_JobApplication_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterviewParticipant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewParticipant_Interview_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewParticipant_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_InterviewId",
                table: "Feedback",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Interview_JobApplicationId",
                table: "Interview",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewParticipant_InterviewId",
                table: "InterviewParticipant",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewParticipant_UserId",
                table: "InterviewParticipant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Interview_InterviewId",
                table: "Feedback",
                column: "InterviewId",
                principalTable: "Interview",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Interview_InterviewId",
                table: "Feedback");

            migrationBuilder.DropTable(
                name: "InterviewParticipant");

            migrationBuilder.DropTable(
                name: "Interview");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_InterviewId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "InterviewId",
                table: "Feedback");
        }
    }
}