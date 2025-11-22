using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemainingTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Role",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Role",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Role",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Role",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Role",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Role",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Role",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DocumentType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DocumentType",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DocumentType",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "DocumentType",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DocumentType",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "DocumentType",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdatedBy",
                table: "DocumentType",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedBy",
                table: "Role",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_DeletedBy",
                table: "Role",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedBy",
                table: "Role",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_CreatedBy",
                table: "DocumentType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_DeletedBy",
                table: "DocumentType",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentType_LastUpdatedBy",
                table: "DocumentType",
                column: "LastUpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentType_User_CreatedBy",
                table: "DocumentType",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentType_User_DeletedBy",
                table: "DocumentType",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentType_User_LastUpdatedBy",
                table: "DocumentType",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_CreatedBy",
                table: "Role",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_DeletedBy",
                table: "Role",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_User_LastUpdatedBy",
                table: "Role",
                column: "LastUpdatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentType_User_CreatedBy",
                table: "DocumentType");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentType_User_DeletedBy",
                table: "DocumentType");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentType_User_LastUpdatedBy",
                table: "DocumentType");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_CreatedBy",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_DeletedBy",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_User_LastUpdatedBy",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_CreatedBy",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_DeletedBy",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_LastUpdatedBy",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_DocumentType_CreatedBy",
                table: "DocumentType");

            migrationBuilder.DropIndex(
                name: "IX_DocumentType_DeletedBy",
                table: "DocumentType");

            migrationBuilder.DropIndex(
                name: "IX_DocumentType_LastUpdatedBy",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "DocumentType");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "DocumentType");
        }
    }
}