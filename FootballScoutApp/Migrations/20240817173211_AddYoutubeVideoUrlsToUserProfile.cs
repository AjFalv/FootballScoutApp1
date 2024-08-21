using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballScoutApp.Migrations
{
    /// <inheritdoc />
    public partial class AddYoutubeVideoUrlsToUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeVideoUrl",
                table: "UserProfiles",
                newName: "YoutubeVideoUrlsJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "YoutubeVideoUrlsJson",
                table: "UserProfiles",
                newName: "YoutubeVideoUrl");
        }
    }
}
