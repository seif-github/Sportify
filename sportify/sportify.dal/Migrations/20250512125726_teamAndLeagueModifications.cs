using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sportify.DAL.Migrations
{
    /// <inheritdoc />
    public partial class teamAndLeagueModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SportType",
                table: "Leagues");

            migrationBuilder.AddColumn<int>(
                name: "GoalsConceded",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsScored",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalsConceded",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GoalsScored",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "SportType",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
