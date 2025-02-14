using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class NewerVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_project_assignment_task_ProjectTaskId",
                table: "project_assignment");

            migrationBuilder.DropIndex(
                name: "IX_project_assignment_ProjectTaskId",
                table: "project_assignment");

            migrationBuilder.DropIndex(
                name: "IX_contacts_user_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "project_assignment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tag",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TagName",
                table: "tag",
                newName: "name");

            migrationBuilder.AddColumn<DateTime>(
                name: "assigned_at",
                table: "project_assignment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    task_id = table.Column<int>(type: "integer", nullable: false),
                    commentedTaskId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_task_commentedTaskId",
                        column: x => x.commentedTaskId,
                        principalTable: "task",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comment_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_assignment_task_id",
                table: "project_assignment",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_user_id",
                table: "contacts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_commentedTaskId",
                table: "comment",
                column: "commentedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_project_assignment_task_task_id",
                table: "project_assignment",
                column: "task_id",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_project_assignment_task_task_id",
                table: "project_assignment");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropIndex(
                name: "IX_project_assignment_task_id",
                table: "project_assignment");

            migrationBuilder.DropIndex(
                name: "IX_contacts_user_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "assigned_at",
                table: "project_assignment");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tag",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "tag",
                newName: "TagName");

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "project_assignment",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_assignment_ProjectTaskId",
                table: "project_assignment",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_user_id",
                table: "contacts",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_project_assignment_task_ProjectTaskId",
                table: "project_assignment",
                column: "ProjectTaskId",
                principalTable: "task",
                principalColumn: "id");
        }
    }
}
