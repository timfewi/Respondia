using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Respondia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SubmitCountToFeedbackForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmitCount",
                table: "FeedbackForms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitCount",
                table: "FeedbackForms");
        }
    }
}
