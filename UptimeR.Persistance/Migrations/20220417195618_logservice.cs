using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UptimeR.Persistance.Migrations
{
    public partial class logservice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsedHttp",
                table: "LogHistorys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsedPing",
                table: "LogHistorys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedHttp",
                table: "LogHistorys");

            migrationBuilder.DropColumn(
                name: "UsedPing",
                table: "LogHistorys");
        }
    }
}
