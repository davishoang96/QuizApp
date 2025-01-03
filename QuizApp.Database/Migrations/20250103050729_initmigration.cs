using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "BaseModel");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "BaseModel",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "BaseModel",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "ChoiceId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChoiceTitle",
                table: "BaseModel",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Choice_QuestionId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BaseModel",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId1",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizName",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubmissionId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Submission_QuizId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BaseModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BaseModel",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseModel",
                table: "BaseModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_Choice_QuestionId",
                table: "BaseModel",
                column: "Choice_QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_ChoiceId",
                table: "BaseModel",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_QuestionId",
                table: "BaseModel",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_QuestionId1",
                table: "BaseModel",
                column: "QuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_QuizId",
                table: "BaseModel",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_Submission_QuizId",
                table: "BaseModel",
                column: "Submission_QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_SubmissionId",
                table: "BaseModel",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseModel_UserId",
                table: "BaseModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_ChoiceId",
                table: "BaseModel",
                column: "ChoiceId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_Choice_QuestionId",
                table: "BaseModel",
                column: "Choice_QuestionId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_QuestionId",
                table: "BaseModel",
                column: "QuestionId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_QuestionId1",
                table: "BaseModel",
                column: "QuestionId1",
                principalTable: "BaseModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_QuizId",
                table: "BaseModel",
                column: "QuizId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_SubmissionId",
                table: "BaseModel",
                column: "SubmissionId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_Submission_QuizId",
                table: "BaseModel",
                column: "Submission_QuizId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseModel_BaseModel_UserId",
                table: "BaseModel",
                column: "UserId",
                principalTable: "BaseModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_ChoiceId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_Choice_QuestionId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_QuestionId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_QuestionId1",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_QuizId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_SubmissionId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_Submission_QuizId",
                table: "BaseModel");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseModel_BaseModel_UserId",
                table: "BaseModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseModel",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_Choice_QuestionId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_ChoiceId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_QuestionId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_QuestionId1",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_QuizId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_Submission_QuizId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_SubmissionId",
                table: "BaseModel");

            migrationBuilder.DropIndex(
                name: "IX_BaseModel_UserId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "ChoiceId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "ChoiceTitle",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Choice_QuestionId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "QuestionId1",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "QuizName",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "SubmissionId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Submission_QuizId",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BaseModel");

            migrationBuilder.RenameTable(
                name: "BaseModel",
                newName: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChoiceTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Choices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "QuizName" },
                values: new object[,]
                {
                    { 1, "General Knowledge" },
                    { 2, "Science Trivia" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "QuizId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "What is the capital of France?" },
                    { 2, 1, "Who wrote 'Romeo and Juliet'?" },
                    { 3, 2, "What is the chemical symbol for water?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_ChoiceId",
                table: "Answers",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId",
                table: "Answers",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_QuizId",
                table: "Submissions",
                column: "QuizId");
        }
    }
}
