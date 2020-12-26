using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KhatmaBackEnd.Migrations
{
    public partial class IntaiteDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "khatmas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khatmas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userPages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageNo = table.Column<int>(nullable: true),
                    IRead = table.Column<bool>(nullable: true),
                    PageDistributedDate = table.Column<DateTime>(nullable: true),
                    ReadedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KhatmaSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KhatmaCount = table.Column<int>(nullable: false),
                    LastDistributedPage = table.Column<int>(nullable: true),
                    KhatmaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhatmaSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KhatmaSettings_khatmas_KhatmaId",
                        column: x => x.KhatmaId,
                        principalTable: "khatmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    KhatmaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_khatmas_KhatmaId",
                        column: x => x.KhatmaId,
                        principalTable: "khatmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: false),
                    FName = table.Column<string>(nullable: true),
                    LName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    PageNo = table.Column<int>(nullable: true),
                    IsRead = table.Column<bool>(nullable: true),
                    PageDistributedDate = table.Column<DateTime>(nullable: true),
                    ReadedDate = table.Column<DateTime>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    KhatmaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_khatmas_KhatmaId",
                        column: x => x.KhatmaId,
                        principalTable: "khatmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userDevices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    DeviceToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userDevices_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "khatmas",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[] { 1, "Public", 0 });

            migrationBuilder.InsertData(
                table: "KhatmaSettings",
                columns: new[] { "Id", "KhatmaCount", "KhatmaId", "LastDistributedPage" },
                values: new object[] { 1, 0, 1, 0 });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "Id", "KhatmaId", "Name" },
                values: new object[] { 1, 1, "Public" });

            migrationBuilder.CreateIndex(
                name: "IX_KhatmaSettings_KhatmaId",
                table: "KhatmaSettings",
                column: "KhatmaId");

            migrationBuilder.CreateIndex(
                name: "IX_userDevices_UserID",
                table: "userDevices",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_KhatmaId",
                table: "UserGroups",
                column: "KhatmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_KhatmaId",
                table: "Users",
                column: "KhatmaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KhatmaSettings");

            migrationBuilder.DropTable(
                name: "userDevices");

            migrationBuilder.DropTable(
                name: "userPages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "khatmas");
        }
    }
}
