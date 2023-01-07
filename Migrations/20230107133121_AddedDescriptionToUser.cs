using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedditNet.Migrations
{
    public partial class AddedDescriptionToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profilePic",
                table: "AspNetUsers",
                newName: "ProfilePic");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePic",
                table: "AspNetUsers",
                newName: "profilePic");
        }
    }
}
