using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Respondia.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IsFinalizedBoolToForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "FeedbackForms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "FeedbackForms");
        }
    }
}
