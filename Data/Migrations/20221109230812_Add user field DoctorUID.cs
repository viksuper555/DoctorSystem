using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorSystem.Data.Migrations
{
    public partial class AdduserfieldDoctorUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorUID",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorUID",
                table: "AspNetUsers");
        }
    }
}
