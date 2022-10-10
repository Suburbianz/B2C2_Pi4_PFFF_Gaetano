using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2_Pi4_PFFF_Gaetano.Migrations
{
    public partial class ShareUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShareUserName",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareUserName",
                table: "AspNetUsers");
        }
    }
}
