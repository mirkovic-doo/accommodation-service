using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationService.Migrations
{
    /// <inheritdoc />
    public partial class TableNamesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityPeriod_Properties_PropertyId",
                table: "AvailabilityPeriod");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Properties_PropertyId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityPeriod",
                table: "AvailabilityPeriod");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "AvailabilityPeriod",
                newName: "AvailabilityPeriods");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_PropertyId",
                table: "Reservations",
                newName: "IX_Reservations_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityPeriod_PropertyId",
                table: "AvailabilityPeriods",
                newName: "IX_AvailabilityPeriods_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityPeriods",
                table: "AvailabilityPeriods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityPeriods_Properties_PropertyId",
                table: "AvailabilityPeriods",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Properties_PropertyId",
                table: "Reservations",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityPeriods_Properties_PropertyId",
                table: "AvailabilityPeriods");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Properties_PropertyId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityPeriods",
                table: "AvailabilityPeriods");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "AvailabilityPeriods",
                newName: "AvailabilityPeriod");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_PropertyId",
                table: "Reservation",
                newName: "IX_Reservation_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityPeriods_PropertyId",
                table: "AvailabilityPeriod",
                newName: "IX_AvailabilityPeriod_PropertyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityPeriod",
                table: "AvailabilityPeriod",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityPeriod_Properties_PropertyId",
                table: "AvailabilityPeriod",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Properties_PropertyId",
                table: "Reservation",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
