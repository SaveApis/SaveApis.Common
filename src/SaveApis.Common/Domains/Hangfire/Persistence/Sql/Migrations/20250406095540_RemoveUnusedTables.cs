using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveApis.Common.Domains.Hangfire.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobConfigurations",
                schema: "Hangfire");

            migrationBuilder.DropTable(
                name: "RecurringEvents",
                schema: "Hangfire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Hangfire");

            migrationBuilder.CreateTable(
                name: "JobConfigurations",
                schema: "Hangfire",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Namespace = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobConfigurations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RecurringEvents",
                schema: "Hangfire",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cron = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Key = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringEvents", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JobConfigurations_Namespace_Name",
                schema: "Hangfire",
                table: "JobConfigurations",
                columns: new[] { "Namespace", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecurringEvents_Key",
                schema: "Hangfire",
                table: "RecurringEvents",
                column: "Key",
                unique: true);
        }
    }
}
