using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteDesktopApplication.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServerHost",
                table: "servermanager",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerHost",
                table: "servermanager");
        }
    }
}
