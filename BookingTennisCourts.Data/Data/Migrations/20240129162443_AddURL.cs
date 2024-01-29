using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTennisCourts.Migrations
{
    /// <inheritdoc />
    public partial class AddURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Courts_CourtId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "CourtId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlToPicture",
                table: "Courts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Courts_CourtId",
                table: "Reservations",
                column: "CourtId",
                principalTable: "Courts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Courts_CourtId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "UrlToPicture",
                table: "Courts");

            migrationBuilder.AlterColumn<int>(
                name: "CourtId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Courts_CourtId",
                table: "Reservations",
                column: "CourtId",
                principalTable: "Courts",
                principalColumn: "Id");
        }
    }
}
