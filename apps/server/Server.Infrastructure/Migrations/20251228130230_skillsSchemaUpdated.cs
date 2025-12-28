using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class skillsSchemaUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("010586be-8483-4cbd-932d-b786ca51f3fd"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("084fae9a-d674-4e84-81f2-ef028625b31f"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("258d59d0-f98d-445d-9938-a0baea01b04d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("694962eb-7fc0-42c6-8e20-96cf736cba25"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("741a84df-76e7-4293-8b51-f92159e8b023"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7ebf0dcd-df29-4c68-856b-48852fe5e580"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("a381734a-5fd3-439c-a264-0dc153f6d281"));

            migrationBuilder.DropColumn(
                name: "MinExperienceYears",
                table: "SkillOverRide");

            migrationBuilder.DropColumn(
                name: "MinExperienceYears",
                table: "DesignationSkill");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "LastUpdatedAt", "LastUpdatedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("2508f54e-ab1f-481c-859f-4f1505533dfa"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5397), null, null, null, "Culture fit, final negotiation, documentation and background verification", null, null, "HR" },
                    { new Guid("730a5c0e-76a2-4826-a192-3c584a367f82"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5369), null, null, null, "Manages users, roles, and system-wide configurations.", null, null, "Admin" },
                    { new Guid("761a2beb-ae68-4950-865b-98ea7eb7079a"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5399), null, null, null, "Screens CVs and shortlists candidates.", null, null, "Reviewer" },
                    { new Guid("78e3afbe-c2b2-459b-a114-1f23e4cd42f0"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5402), null, null, null, "Views job openings, uploads CVs, and submits documents.", null, null, "Candidate" },
                    { new Guid("8fd55f0f-8262-4781-ba7b-7124db6f147b"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5404), null, null, null, "Read-only access to all data.", null, null, "Viewer" },
                    { new Guid("d51ae472-657e-4b68-9c73-82be537ae083"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5377), null, null, null, "Manages job openings, candidate profiles, interviews.", null, null, "Recruiter" },
                    { new Guid("e8157fe7-7c8e-44ad-8440-891350dbc7e9"), new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5381), null, null, null, "Provides interview feedback.", null, null, "Interviewer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2508f54e-ab1f-481c-859f-4f1505533dfa"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("730a5c0e-76a2-4826-a192-3c584a367f82"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("761a2beb-ae68-4950-865b-98ea7eb7079a"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("78e3afbe-c2b2-459b-a114-1f23e4cd42f0"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8fd55f0f-8262-4781-ba7b-7124db6f147b"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d51ae472-657e-4b68-9c73-82be537ae083"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("e8157fe7-7c8e-44ad-8440-891350dbc7e9"));

            migrationBuilder.AddColumn<float>(
                name: "MinExperienceYears",
                table: "SkillOverRide",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "MinExperienceYears",
                table: "DesignationSkill",
                type: "real",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "LastUpdatedAt", "LastUpdatedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("010586be-8483-4cbd-932d-b786ca51f3fd"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(29), null, null, null, "Read-only access to all data.", null, null, "Viewer" },
                    { new Guid("084fae9a-d674-4e84-81f2-ef028625b31f"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(12), null, null, null, "Manages users, roles, and system-wide configurations.", null, null, "Admin" },
                    { new Guid("258d59d0-f98d-445d-9938-a0baea01b04d"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(23), null, null, null, "Provides interview feedback.", null, null, "Interviewer" },
                    { new Guid("694962eb-7fc0-42c6-8e20-96cf736cba25"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(28), null, null, null, "Views job openings, uploads CVs, and submits documents.", null, null, "Candidate" },
                    { new Guid("741a84df-76e7-4293-8b51-f92159e8b023"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(26), null, null, null, "Screens CVs and shortlists candidates.", null, null, "Reviewer" },
                    { new Guid("7ebf0dcd-df29-4c68-856b-48852fe5e580"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(24), null, null, null, "Culture fit, final negotiation, documentation and background verification", null, null, "HR" },
                    { new Guid("a381734a-5fd3-439c-a264-0dc153f6d281"), new DateTime(2025, 12, 23, 16, 33, 31, 449, DateTimeKind.Utc).AddTicks(21), null, null, null, "Manages job openings, candidate profiles, interviews.", null, null, "Recruiter" }
                });
        }
    }
}