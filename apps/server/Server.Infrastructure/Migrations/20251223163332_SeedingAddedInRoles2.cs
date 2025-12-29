using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAddedInRoles2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("04ba6d0a-cdad-4152-b731-2492daf99031"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3f8be536-933a-4e1b-b1f0-c21b64580edd"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("58cc596f-c36f-4ab2-8f94-5d8f932c3dc4"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("68739ad8-2d01-4f10-91e9-32e8f91bcdb4"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("6a28cbe8-d7cd-487f-a07e-432668248501"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "Description", "LastUpdatedAt", "LastUpdatedBy", "Name" },
                values: new object[,]
                {
                    { new Guid("04ba6d0a-cdad-4152-b731-2492daf99031"), new DateTime(2025, 12, 23, 16, 5, 23, 240, DateTimeKind.Utc).AddTicks(1135), null, null, null, "Manages job openings, candidate profiles, interviews", null, null, "Recruiter" },
                    { new Guid("3f8be536-933a-4e1b-b1f0-c21b64580edd"), new DateTime(2025, 12, 23, 16, 5, 23, 240, DateTimeKind.Utc).AddTicks(1140), null, null, null, "culture fit, final negotiation, documentation and background verification", null, null, "HR" },
                    { new Guid("58cc596f-c36f-4ab2-8f94-5d8f932c3dc4"), new DateTime(2025, 12, 23, 16, 5, 23, 240, DateTimeKind.Utc).AddTicks(1143), null, null, null, "Screens CVs and shortlists candidates.", null, null, "Reviewer" },
                    { new Guid("68739ad8-2d01-4f10-91e9-32e8f91bcdb4"), new DateTime(2025, 12, 23, 16, 5, 23, 240, DateTimeKind.Utc).AddTicks(1127), null, null, null, "Manages users, roles, and system-wide configurations.", null, null, "Admin" },
                    { new Guid("6a28cbe8-d7cd-487f-a07e-432668248501"), new DateTime(2025, 12, 23, 16, 5, 23, 240, DateTimeKind.Utc).AddTicks(1138), null, null, null, "Provides interview feedback.", null, null, "Interviewer" }
                });
        }
    }
}