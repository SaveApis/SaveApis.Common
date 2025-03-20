using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveApis.Common.Persistence.Sql.Tracking.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntryValuesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Entries_TrackingEntryId",
                schema: "Tracking",
                table: "Values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Values",
                schema: "Tracking",
                table: "Values");

            migrationBuilder.RenameTable(
                name: "Values",
                schema: "Tracking",
                newName: "EntryValues",
                newSchema: "Tracking");

            migrationBuilder.RenameIndex(
                name: "IX_Values_TrackingEntryId",
                schema: "Tracking",
                table: "EntryValues",
                newName: "IX_EntryValues_TrackingEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntryValues",
                schema: "Tracking",
                table: "EntryValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntryValues_Entries_TrackingEntryId",
                schema: "Tracking",
                table: "EntryValues",
                column: "TrackingEntryId",
                principalSchema: "Tracking",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntryValues_Entries_TrackingEntryId",
                schema: "Tracking",
                table: "EntryValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntryValues",
                schema: "Tracking",
                table: "EntryValues");

            migrationBuilder.RenameTable(
                name: "EntryValues",
                schema: "Tracking",
                newName: "Values",
                newSchema: "Tracking");

            migrationBuilder.RenameIndex(
                name: "IX_EntryValues_TrackingEntryId",
                schema: "Tracking",
                table: "Values",
                newName: "IX_Values_TrackingEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Values",
                schema: "Tracking",
                table: "Values",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Entries_TrackingEntryId",
                schema: "Tracking",
                table: "Values",
                column: "TrackingEntryId",
                principalSchema: "Tracking",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
