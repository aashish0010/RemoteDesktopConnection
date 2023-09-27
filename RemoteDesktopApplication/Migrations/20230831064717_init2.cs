using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoteDesktopApplication.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "siteManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SiteLink = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SiteRoot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SiteType = table.Column<string>(type: "nvarchar(130)", maxLength: 130, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_siteManagers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "siteManagers");
        }
    }
}
