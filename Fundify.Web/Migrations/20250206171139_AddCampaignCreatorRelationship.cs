using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fundify.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignCreatorRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Campaigns_CampaignId1",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_CampaignId1",
                table: "Rewards");

            migrationBuilder.DropColumn(
                name: "CampaignId1",
                table: "Rewards");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Campaigns",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId1",
                table: "Rewards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Campaigns",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_CampaignId1",
                table: "Rewards",
                column: "CampaignId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Campaigns_CampaignId1",
                table: "Rewards",
                column: "CampaignId1",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
