using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAddedInRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}