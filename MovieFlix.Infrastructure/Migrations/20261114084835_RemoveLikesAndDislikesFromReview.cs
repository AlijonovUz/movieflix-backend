using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieFlix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLikesAndDislikesFromReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikesCount",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikesCount",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
