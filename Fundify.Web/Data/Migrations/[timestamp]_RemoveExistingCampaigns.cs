using Microsoft.EntityFrameworkCore.Migrations;

public partial class RemoveExistingCampaigns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM Campaigns");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // No down migration needed since we can't restore deleted data
    }
} 