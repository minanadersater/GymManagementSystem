using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class all : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Member_MemberId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Member_MemberId",
                table: "HealthRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Membership_Member_MemberId",
                table: "Membership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "Members");

            migrationBuilder.RenameIndex(
                name: "IX_Member_Phone",
                table: "Members",
                newName: "IX_Members_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Member_Email",
                table: "Members",
                newName: "IX_Members_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Members_MemberId",
                table: "Booking",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_Members_MemberId",
                table: "HealthRecord",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_Members_MemberId",
                table: "Membership",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Members_MemberId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecord_Members_MemberId",
                table: "HealthRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Membership_Members_MemberId",
                table: "Membership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Member");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Phone",
                table: "Member",
                newName: "IX_Member_Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Email",
                table: "Member",
                newName: "IX_Member_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Member_MemberId",
                table: "Booking",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecord_Member_MemberId",
                table: "HealthRecord",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_Member_MemberId",
                table: "Membership",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
