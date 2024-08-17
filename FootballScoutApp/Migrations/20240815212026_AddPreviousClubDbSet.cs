using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballScoutApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPreviousClubDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousClub_UserProfiles_UserProfileId",
                table: "PreviousClub");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousClub",
                table: "PreviousClub");

            migrationBuilder.RenameTable(
                name: "PreviousClub",
                newName: "PreviousClubs");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousClub_UserProfileId",
                table: "PreviousClubs",
                newName: "IX_PreviousClubs_UserProfileId");

            migrationBuilder.AlterColumn<string>(
                name: "Goals",
                table: "PreviousClubs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CleanSheets",
                table: "PreviousClubs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Appearances",
                table: "PreviousClubs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousClubs",
                table: "PreviousClubs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousClubs_UserProfiles_UserProfileId",
                table: "PreviousClubs",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreviousClubs_UserProfiles_UserProfileId",
                table: "PreviousClubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreviousClubs",
                table: "PreviousClubs");

            migrationBuilder.RenameTable(
                name: "PreviousClubs",
                newName: "PreviousClub");

            migrationBuilder.RenameIndex(
                name: "IX_PreviousClubs_UserProfileId",
                table: "PreviousClub",
                newName: "IX_PreviousClub_UserProfileId");

            migrationBuilder.AlterColumn<int>(
                name: "Goals",
                table: "PreviousClub",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CleanSheets",
                table: "PreviousClub",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Appearances",
                table: "PreviousClub",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreviousClub",
                table: "PreviousClub",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousClub_UserProfiles_UserProfileId",
                table: "PreviousClub",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
