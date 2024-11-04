using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewBooks.Migrations
{
    /// <inheritdoc />
    public partial class AddBookReviewQualifying : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qualifying",
                table: "BookReview",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qualifying",
                table: "BookReview");
        }
    }
}
