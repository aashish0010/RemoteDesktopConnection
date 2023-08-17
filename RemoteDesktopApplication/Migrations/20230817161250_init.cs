using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteDesktopApplication.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "servermanager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ServerType = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: true),
                    ServerIpAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ServerUsername = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ServerPassword = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ServerDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servermanager", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "servermanager");
        }
    }
}
