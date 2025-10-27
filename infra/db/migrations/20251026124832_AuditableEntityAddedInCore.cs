using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditableEntityAddedInCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_CreatedBy",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_LastUpdatedBy",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Auth");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedBy",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Skill",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Skill",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Skill",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Skill",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Auth",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Auth",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Auth",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Auth",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Auth",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedBy",
                table: "User",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedBy",
                table: "User",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_DeletedBy",
                table: "Skill",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_CreatedBy",
                table: "Auth",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_DeletedBy",
                table: "Auth",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Auth_LastUpdatedBy",
                table: "Auth",
                column: "LastUpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_User_CreatedBy",
                table: "Auth",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_User_DeletedBy",
                table: "Auth",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Auth_User_LastUpdatedBy",
                table: "Auth",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_CreatedBy",
                table: "Skill",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_DeletedBy",
                table: "Skill",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_LastUpdatedBy",
                table: "Skill",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_DeletedBy",
                table: "User",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_LastUpdatedBy",
                table: "User",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auth_User_CreatedBy",
                table: "Auth");

            migrationBuilder.DropForeignKey(
                name: "FK_Auth_User_DeletedBy",
                table: "Auth");

            migrationBuilder.DropForeignKey(
                name: "FK_Auth_User_LastUpdatedBy",
                table: "Auth");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_CreatedBy",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_DeletedBy",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_LastUpdatedBy",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_DeletedBy",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_LastUpdatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_DeletedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_LastUpdatedBy",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Skill_DeletedBy",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Auth_CreatedBy",
                table: "Auth");

            migrationBuilder.DropIndex(
                name: "IX_Auth_DeletedBy",
                table: "Auth");

            migrationBuilder.DropIndex(
                name: "IX_Auth_LastUpdatedBy",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Auth");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Auth");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Skill",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Auth",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_CreatedBy",
                table: "Skill",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_LastUpdatedBy",
                table: "Skill",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
