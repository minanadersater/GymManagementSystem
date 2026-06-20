using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Trainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Trainer_TrainerId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer");

            migrationBuilder.RenameTable(
                name: "Trainer",
                newName: "Trainers");

            migrationBuilder.RenameIndex(
                name: "IX_Trainer_Phone",
                table: "Trainers",
                newName: "IX_Trainers_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Trainer_Email",
                table: "Trainers",
                newName: "IX_Trainers_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Trainers_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Trainers_TrainerId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainers",
                table: "Trainers");

            migrationBuilder.RenameTable(
                name: "Trainers",
                newName: "Trainer");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_Phone",
                table: "Trainer",
                newName: "IX_Trainer_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_Email",
                table: "Trainer",
                newName: "IX_Trainer_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainer",
                table: "Trainer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Trainer_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "Trainer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
