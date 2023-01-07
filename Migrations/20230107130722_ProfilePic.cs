using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedditNet.Migrations
{
    public partial class ProfilePic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePic",
                table: "AspNetUsers");
        }
    }
}
