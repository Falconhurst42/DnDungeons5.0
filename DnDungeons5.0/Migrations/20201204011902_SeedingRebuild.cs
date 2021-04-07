using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class SeedingRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dungeon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeon", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Enemy",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CR = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    XP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EnemySet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemySet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Layout",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layout", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Loot",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsMoney = table.Column<bool>(type: "bit", nullable: false),
                    MonetaryValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loot", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LootSet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootSet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Filepath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Filetype = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EnemyInSet",
                columns: table => new
                {
                    EnemySetID = table.Column<int>(type: "int", nullable: false),
                    EnemyID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemyInSet", x => new { x.EnemySetID, x.EnemyID });
                    table.ForeignKey(
                        name: "FK_EnemyInSet_Enemy_EnemyID",
                        column: x => x.EnemyID,
                        principalTable: "Enemy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnemyInSet_EnemySet_EnemySetID",
                        column: x => x.EnemySetID,
                        principalTable: "EnemySet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    DungeonID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LayoutID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => new { x.RoomNumber, x.DungeonID });
                    table.ForeignKey(
                        name: "FK_Room_Dungeon_DungeonID",
                        column: x => x.DungeonID,
                        principalTable: "Dungeon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Room_Layout_LayoutID",
                        column: x => x.LayoutID,
                        principalTable: "Layout",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LootInSet",
                columns: table => new
                {
                    LootSetID = table.Column<int>(type: "int", nullable: false),
                    LootID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootInSet", x => new { x.LootSetID, x.LootID });
                    table.ForeignKey(
                        name: "FK_LootInSet_Loot_LootID",
                        column: x => x.LootID,
                        principalTable: "Loot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LootInSet_LootSet_LootSetID",
                        column: x => x.LootSetID,
                        principalTable: "LootSet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnemyMedia",
                columns: table => new
                {
                    EnemyID = table.Column<int>(type: "int", nullable: false),
                    MediaID = table.Column<int>(type: "int", nullable: false),
                    MediaLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemyMedia", x => new { x.EnemyID, x.MediaID });
                    table.ForeignKey(
                        name: "FK_EnemyMedia_Enemy_EnemyID",
                        column: x => x.EnemyID,
                        principalTable: "Enemy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnemyMedia_Media_MediaID",
                        column: x => x.MediaID,
                        principalTable: "Media",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LayoutMedia",
                columns: table => new
                {
                    LayoutID = table.Column<int>(type: "int", nullable: false),
                    MediaID = table.Column<int>(type: "int", nullable: false),
                    MediaLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutMedia", x => new { x.LayoutID, x.MediaID });
                    table.ForeignKey(
                        name: "FK_LayoutMedia_Layout_LayoutID",
                        column: x => x.LayoutID,
                        principalTable: "Layout",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LayoutMedia_Media_MediaID",
                        column: x => x.MediaID,
                        principalTable: "Media",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LootMedia",
                columns: table => new
                {
                    LootID = table.Column<int>(type: "int", nullable: false),
                    MediaID = table.Column<int>(type: "int", nullable: false),
                    MediaLabel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootMedia", x => new { x.LootID, x.MediaID });
                    table.ForeignKey(
                        name: "FK_LootMedia_Loot_LootID",
                        column: x => x.LootID,
                        principalTable: "Loot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LootMedia_Media_MediaID",
                        column: x => x.MediaID,
                        principalTable: "Media",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnemyInRoom",
                columns: table => new
                {
                    DungeonID = table.Column<int>(type: "int", nullable: false),
                    RoomNum = table.Column<int>(type: "int", nullable: false),
                    EnemyID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoomNumber = table.Column<int>(type: "int", nullable: true),
                    RoomDungeonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemyInRoom", x => new { x.DungeonID, x.RoomNum, x.EnemyID });
                    table.ForeignKey(
                        name: "FK_EnemyInRoom_Enemy_EnemyID",
                        column: x => x.EnemyID,
                        principalTable: "Enemy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnemyInRoom_Room_RoomNumber_RoomDungeonID",
                        columns: x => new { x.RoomNumber, x.RoomDungeonID },
                        principalTable: "Room",
                        principalColumns: new[] { "RoomNumber", "DungeonID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LootInRoom",
                columns: table => new
                {
                    DungeonID = table.Column<int>(type: "int", nullable: false),
                    RoomNum = table.Column<int>(type: "int", nullable: false),
                    LootID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RoomNumber = table.Column<int>(type: "int", nullable: true),
                    RoomDungeonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootInRoom", x => new { x.DungeonID, x.RoomNum, x.LootID });
                    table.ForeignKey(
                        name: "FK_LootInRoom_Loot_LootID",
                        column: x => x.LootID,
                        principalTable: "Loot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LootInRoom_Room_RoomNumber_RoomDungeonID",
                        columns: x => new { x.RoomNumber, x.RoomDungeonID },
                        principalTable: "Room",
                        principalColumns: new[] { "RoomNumber", "DungeonID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomConnection",
                columns: table => new
                {
                    DungeonID = table.Column<int>(type: "int", nullable: false),
                    Room1Num = table.Column<int>(type: "int", nullable: false),
                    Room2Num = table.Column<int>(type: "int", nullable: false),
                    ConnectionPoint1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConnectionPoint2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Room1RoomNumber = table.Column<int>(type: "int", nullable: true),
                    Room1DungeonID = table.Column<int>(type: "int", nullable: true),
                    Room2RoomNumber = table.Column<int>(type: "int", nullable: true),
                    Room2DungeonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomConnection", x => new { x.DungeonID, x.Room1Num, x.Room2Num });
                    table.ForeignKey(
                        name: "FK_RoomConnection_Dungeon_DungeonID",
                        column: x => x.DungeonID,
                        principalTable: "Dungeon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomConnection_Room_Room1RoomNumber_Room1DungeonID",
                        columns: x => new { x.Room1RoomNumber, x.Room1DungeonID },
                        principalTable: "Room",
                        principalColumns: new[] { "RoomNumber", "DungeonID" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomConnection_Room_Room2RoomNumber_Room2DungeonID",
                        columns: x => new { x.Room2RoomNumber, x.Room2DungeonID },
                        principalTable: "Room",
                        principalColumns: new[] { "RoomNumber", "DungeonID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnemyInRoom_EnemyID",
                table: "EnemyInRoom",
                column: "EnemyID");

            migrationBuilder.CreateIndex(
                name: "IX_EnemyInRoom_RoomNumber_RoomDungeonID",
                table: "EnemyInRoom",
                columns: new[] { "RoomNumber", "RoomDungeonID" });

            migrationBuilder.CreateIndex(
                name: "IX_EnemyInSet_EnemyID",
                table: "EnemyInSet",
                column: "EnemyID");

            migrationBuilder.CreateIndex(
                name: "IX_EnemyMedia_MediaID",
                table: "EnemyMedia",
                column: "MediaID");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutMedia_MediaID",
                table: "LayoutMedia",
                column: "MediaID");

            migrationBuilder.CreateIndex(
                name: "IX_LootInRoom_LootID",
                table: "LootInRoom",
                column: "LootID");

            migrationBuilder.CreateIndex(
                name: "IX_LootInRoom_RoomNumber_RoomDungeonID",
                table: "LootInRoom",
                columns: new[] { "RoomNumber", "RoomDungeonID" });

            migrationBuilder.CreateIndex(
                name: "IX_LootInSet_LootID",
                table: "LootInSet",
                column: "LootID");

            migrationBuilder.CreateIndex(
                name: "IX_LootMedia_MediaID",
                table: "LootMedia",
                column: "MediaID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_DungeonID",
                table: "Room",
                column: "DungeonID");

            migrationBuilder.CreateIndex(
                name: "IX_Room_LayoutID",
                table: "Room",
                column: "LayoutID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomConnection_Room1RoomNumber_Room1DungeonID",
                table: "RoomConnection",
                columns: new[] { "Room1RoomNumber", "Room1DungeonID" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomConnection_Room2RoomNumber_Room2DungeonID",
                table: "RoomConnection",
                columns: new[] { "Room2RoomNumber", "Room2DungeonID" });

            migrationBuilder.Sql("CREATE TRIGGER dbo.dungeon_lu_update ON dbo.Dungeon AFTER UPDATE AS BEGIN UPDATE dbo.Dungeon SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnemyInRoom");

            migrationBuilder.DropTable(
                name: "EnemyInSet");

            migrationBuilder.DropTable(
                name: "EnemyMedia");

            migrationBuilder.DropTable(
                name: "LayoutMedia");

            migrationBuilder.DropTable(
                name: "LootInRoom");

            migrationBuilder.DropTable(
                name: "LootInSet");

            migrationBuilder.DropTable(
                name: "LootMedia");

            migrationBuilder.DropTable(
                name: "RoomConnection");

            migrationBuilder.DropTable(
                name: "EnemySet");

            migrationBuilder.DropTable(
                name: "Enemy");

            migrationBuilder.DropTable(
                name: "LootSet");

            migrationBuilder.DropTable(
                name: "Loot");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Dungeon");

            migrationBuilder.DropTable(
                name: "Layout");

            migrationBuilder.Sql("DROP TRIGGER dbo.dungeon_lu_update");
        }
    }
}
