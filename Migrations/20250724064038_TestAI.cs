using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoEASy.Migrations
{
    /// <inheritdoc />
    public partial class TestAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__Users__A9D1053449FFE89D",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UQ__BlogTags__737584F6AEFE61A6",
                table: "BlogTags");

            migrationBuilder.DropIndex(
                name: "UQ__Admins__A9D1053452D3FF2D",
                table: "Admins");

            migrationBuilder.CreateTable(
                name: "SemanticQueries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Query = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TopK = table.Column<int>(type: "int", nullable: false, defaultValue: 5)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Semantic__3214EC27CD216E46", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TourDtos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourDtos__3214EC27758DAF54", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TourViewLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ViewTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TourView__3214EC27460BF978", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D1053449FFE89D",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "([Email] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "UQ__BlogTags__737584F6AEFE61A6",
                table: "BlogTags",
                column: "Name",
                unique: true,
                filter: "([Name] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "UQ__Admins__A9D1053452D3FF2D",
                table: "Admins",
                column: "Email",
                unique: true,
                filter: "([Email] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SemanticQueries");

            migrationBuilder.DropTable(
                name: "TourDtos");

            migrationBuilder.DropTable(
                name: "TourViewLogs");

            migrationBuilder.DropIndex(
                name: "UQ__Users__A9D1053449FFE89D",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UQ__BlogTags__737584F6AEFE61A6",
                table: "BlogTags");

            migrationBuilder.DropIndex(
                name: "UQ__Admins__A9D1053452D3FF2D",
                table: "Admins");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D1053449FFE89D",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__BlogTags__737584F6AEFE61A6",
                table: "BlogTags",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Admins__A9D1053452D3FF2D",
                table: "Admins",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }
    }
}
