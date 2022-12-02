using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedditNet.Migrations
{
    /// <inheritdoc />
    public partial class SubRedditMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubReddits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubReddits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubReddits_Id",
                table: "SubReddits",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubReddits");
        }
    }
}
