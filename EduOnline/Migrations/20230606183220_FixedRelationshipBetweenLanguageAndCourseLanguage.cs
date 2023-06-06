using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduOnline.Migrations
{
    /// <inheritdoc />
    public partial class FixedRelationshipBetweenLanguageAndCourseLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Languages_LanguageId",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_LanguageId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Languages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Languages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_LanguageId",
                table: "Languages",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Languages_LanguageId",
                table: "Languages",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }
    }
}
