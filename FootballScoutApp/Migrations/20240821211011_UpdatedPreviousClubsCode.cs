using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballScoutApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPreviousClubsCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreviousClubs");

            migrationBuilder.RenameColumn(
                name: "PreviousClubsJson",
                table: "UserProfiles",
                newName: "PreviousClub3Name");

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub1Appearances",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub1CleanSheets",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub1Goals",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousClub1Name",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub1YearFrom",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub1YearTo",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub2Appearances",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub2CleanSheets",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub2Goals",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousClub2Name",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub2YearFrom",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub2YearTo",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub3Appearances",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub3CleanSheets",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub3Goals",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub3YearFrom",
                table: "UserProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousClub3YearTo",
                table: "UserProfiles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousClub1Appearances",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub1CleanSheets",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub1Goals",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub1Name",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub1YearFrom",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub1YearTo",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2Appearances",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2CleanSheets",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2Goals",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2Name",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2YearFrom",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub2YearTo",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub3Appearances",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub3CleanSheets",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub3Goals",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub3YearFrom",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "PreviousClub3YearTo",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "PreviousClub3Name",
                table: "UserProfiles",
                newName: "PreviousClubsJson");

            migrationBuilder.CreateTable(
                name: "PreviousClubs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Appearances = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CleanSheets = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Goals = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearFrom = table.Column<int>(type: "int", nullable: false),
                    YearTo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousClubs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousClubs_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreviousClubs_UserProfileId",
                table: "PreviousClubs",
                column: "UserProfileId");
        }
    }
}
