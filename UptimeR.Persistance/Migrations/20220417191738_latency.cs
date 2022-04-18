using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UptimeR.Persistance.Migrations
{
    public partial class latency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latency",
                table: "LogHistorys",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latency",
                table: "LogHistorys");
        }
    }
}
