using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2_Pi4_PFFF_Gaetano.Migrations
{
    public partial class AddImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CameraImageUrl",
                table: "CameraReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CameraImageUrl",
                table: "CameraReports");
        }
    }
}
