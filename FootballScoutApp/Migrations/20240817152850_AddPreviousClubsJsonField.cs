using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballScoutApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousClubsJsonField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousClubsJson",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousClubsJson",
                table: "UserProfiles");
        }
    }
}
