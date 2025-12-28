using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rolesSeedProblemResolve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2508f54e-ab1f-481c-859f-4f1505533dfa"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8844));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("730a5c0e-76a2-4826-a192-3c584a367f82"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8816));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("761a2beb-ae68-4950-865b-98ea7eb7079a"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8848));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("78e3afbe-c2b2-459b-a114-1f23e4cd42f0"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8fd55f0f-8262-4781-ba7b-7124db6f147b"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d51ae472-657e-4b68-9c73-82be537ae083"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8826));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("e8157fe7-7c8e-44ad-8440-891350dbc7e9"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 53, 36, 571, DateTimeKind.Utc).AddTicks(8838));

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2508f54e-ab1f-481c-859f-4f1505533dfa"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("730a5c0e-76a2-4826-a192-3c584a367f82"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("761a2beb-ae68-4950-865b-98ea7eb7079a"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("78e3afbe-c2b2-459b-a114-1f23e4cd42f0"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5402));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8fd55f0f-8262-4781-ba7b-7124db6f147b"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5404));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d51ae472-657e-4b68-9c73-82be537ae083"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("e8157fe7-7c8e-44ad-8440-891350dbc7e9"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 13, 2, 29, 183, DateTimeKind.Utc).AddTicks(5381));

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}