using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoomReservation.Data.Migrations
{
    public partial class Rm_Duration_Fr_Reservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndingTime",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingTime",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
