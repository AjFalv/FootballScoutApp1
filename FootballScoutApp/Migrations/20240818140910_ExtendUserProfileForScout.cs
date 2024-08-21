using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballScoutApp.Migrations
{
    /// <inheritdoc />
    public partial class ExtendUserProfileForScout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AchievementsAndAwards",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentAffiliation",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotablePlayersScouted",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryRegions",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScoutingExperience",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoutingPhilosophy",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuccessfulTransfers",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AchievementsAndAwards",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CurrentAffiliation",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "NotablePlayersScouted",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PrimaryRegions",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ScoutingExperience",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ScoutingPhilosophy",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SuccessfulTransfers",
                table: "UserProfiles");
        }
    }
}
