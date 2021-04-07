using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class XPCRLootValCalculations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalMoney",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalValue",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPRating",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPReward",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalMoney",
                table: "LootSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalValue",
                table: "LootSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPRating",
                table: "EnemySet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPReward",
                table: "EnemySet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CRToXP",
                columns: table => new
                {
                    CR = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    XP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EncounterMultiplier",
                columns: table => new
                {
                    CountMin = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(2,1)", nullable: false)
                },
                constraints: table =>
                {
                });

        // propagate tables
            // values in tables taken from Wizard's of the Coast: https://dnd.wizards.com/products/tabletop/dm-basic-rules
            migrationBuilder.InsertData(
                table: "CRToXP",
                columns: new[] { "CR", "XP" },
                values: new object[,] {
                    { 0, 0 },
                    { 0.125, 25 },
                    { 0.25, 50 },
                    { 0.5, 100 },
                    { 1, 200 },
                    { 2, 450 },
                    { 3, 700 },
                    { 4, 1100 },
                    { 5, 1800 },
                    { 6, 2300 },
                    { 7, 2900 },
                    { 8, 3900 },
                    { 9, 5000 },
                    { 10, 5900 },
                    { 11, 7200 },
                    { 12, 8400 },
                    { 13, 10000 },
                    { 14, 11500 },
                    { 15, 13000 },
                    { 16, 15000 },
                    { 17, 18000 },
                    { 18, 20000 },
                    { 19, 22000 },
                    { 20, 25000 },
                    { 21, 33000 },
                    { 22, 41000 },
                    { 23, 50000 },
                    { 24, 62000 },
                    { 25, 75000 },
                    { 26, 90000 },
                    { 27, 105000 },
                    { 28, 120000 },
                    { 29, 135000 },
                    { 30, 155000 }
                });

            migrationBuilder.InsertData(
                table: "EncounterMultiplier",
                columns: new[] { "CountMin", "Multiplier" },
                values: new object[,] {
                    { 1, 1 },
                    { 2, 1.5 },
                    { 3, 2 },
                    { 7, 2.5 },
                    { 11, 3 },
                    { 15, 4 }
                });

        // add indexes to tables
            migrationBuilder.CreateIndex(
                name: "IX_CRToXP_CR",
                table: "CRToXP",
                column: "CR");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterMultiplier_CountMin",
                table: "EncounterMultiplier",
                column: "CountMin");

        // make triggers
            // computed Enemy's XP based on CR whenever inserted/updated
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_xp_compute ON dbo.Enemy " +
                                    "AFTER INSERT, UPDATE " +
                                    "AS " +
                                    "BEGIN " +
                                        "UPDATE dbo.Enemy " +
                                        "SET Enemy.XP = (" +
                                            "SELECT XP " +
                                            "FROM Inserted " +
                                            "WHERE Inserted.CR=Enemy.CR" +
                                        ") " +
                                        "WHERE Enemy.ID IN (SELECT DISTINCT ID FROM Inserted) " +
                                    "END");

            // Generate EnemySet.XPRating, EnemySet.XPReward on inserted/update
            // (note, other triggers will update the LastUpdated of an EnemySet when one of its EIS's is changed which will trigger this query)
            // learned about declaring variables and SELECT INTO from this stackoverflow answers: https://dba.stackexchange.com/a/103081 and https://stackoverflow.com/a/4351314
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_set_xp_compute ON dbo.EnemySet " +
                                    "AFTER INSERT, UPDATE " +
                                    "AS " +
                                    "BEGIN " +
                                        "UPDATE dbo.EnemySet " +
                                        "SET XPReward = ( " +
                                            "" +
                                            "SELECT MAX(Total_XP * Multiplier) " +
                                            "FROM (SELECT * FROM dbo.EncounterMultiplier) AS M " +
                                                "INNER JOIN ( " +
                                                    "SELECT SUM(XP*Count) AS Total_XP, SUM(Count) AS CountXP " +
                                                    "FROM ( " +
                                                        "( SELECT * " +
                                                            "FROM dbo.EnemyInSet " +
                                                            "WHERE EnemySet.ID = EnemyInSet.EnemySetID " +
                                                        ") AS EIS " +
                                                        "INNER JOIN " +
                                                        "dbo.Enemy AS E " +
                                                        "ON E.ID = EIS.EnemyID " +
                                                    ") " +
                                                ") AS XPs ON XPs.CountXP > M.CountMin " +
                                        "), " +
                                        "" +
                                        "XPRating = ( " +
                                            "SELECT SUM(XP*Count) AS Total_XP " +
                                            "FROM ( " +
                                                "( SELECT * " +
                                                    "FROM dbo.EnemyInSet " +
                                                    "WHERE EnemyInSet.EnemySetID = EnemySet.ID ) AS EIS " +
                                                "INNER JOIN " +
                                                "dbo.Enemy AS E " +
                                                "ON E.ID = EIS.EnemyID " +
                                        ") ) " +
                                        "" +
                                        "WHERE EnemySet.ID IN(SELECT DISTINCT ID FROM Inserted) " +
                                        "" +
                                    "END");

            // CREATE TRIGGER dbo.enemy_set_xp_compute ON dbo.EnemySet AFTER INSERT, UPDATE AS BEGIN UPDATE dbo.EnemySet SET XPReward = (SELECT MAX(Total_XP * Multiplier) FROM EncounterMultiplier M INNER JOIN ( SELECT SUM(XP) AS Total_XP, COUNT(XP) AS Count FROM ( SELECT XP FROM ( ( SELECT * FROM dbo.EnemyInSet WHERE EnemySet.ID = EnemyInSet.EnemySetID ) EIS INNER JOIN dbo.Enemy E ON E.ID = EIS.EnemyID ) ) XPs ON XPs.Count > M.CountMin ), XPRating = (SELECT SUM(Total_XP) INTO rating, reward FROM EncounterMultiplier M INNER JOIN ( SELECT SUM(XP) AS Total_XP, COUNT(XP) AS Count FROM ( SELECT XP FROM ( ( SELECT * FROM dbo.EnemyInSet WHERE EnemySet.ID = EnemyInSet.EnemySetID ) EIS INNER JOIN dbo.Enemy E ON E.ID = EIS.EnemyID ) ) XPs ON XPs.Count > M.CountMin) WHERE EnemySet.ID IN(SELECT DISTINCT ID FROM Inserted) END
            // Generate LootSet.TotalValue, EnemySet.TotalMoney on inserted/update
            // (note, other triggers will update the LastUpdated of an LootSet when one of its LIS's is changed which will trigger this query)
            migrationBuilder.Sql("CREATE TRIGGER dbo.loot_set_value_compute ON dbo.LootSet " +
                                    "AFTER INSERT, UPDATE " +
                                    "AS " +
                                    "BEGIN " +
                                        "UPDATE dbo.LootSet " +
                                        "SET TotalValue = (" +
                                            "SELECT SUM(L.MonetaryValue * LIS.Count) " +
                                            "FROM ( ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.LootInSet " +
                                                    "WHERE LootInSet.LootSetID = LootSet.ID " +
                                                ") AS LIS " +
                                                "INNER JOIN " +
                                                "dbo.Loot AS L " +
                                                "ON L.ID = LIS.LootID " +
                                            ") " +
                                        "), " +
                                        "" +
                                        "TotalMoney = ( " +
                                            "SELECT SUM(L.MonetaryValue * LIS.Count) " +
                                            "FROM ( ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.LootInSet " +
                                                    "WHERE LootInSet.LootSetID = LootSet.ID " +
                                                ") AS LIS " +
                                                "INNER JOIN ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.Loot " +
                                                    "WHERE IsMoney = 1 " +
                                                ") AS L " +
                                                "ON L.ID = LIS.LootID " +
                                            ") " +
                                        ") " +
                                        "WHERE LootSet.ID IN(SELECT DISTINCT ID FROM Inserted) " +
                                    "END");

            // copy both of those queries for EnemySets/LootSets for Rooms
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_xp_value_compute ON dbo.Room " +
                                    "AFTER INSERT, UPDATE " +
                                    "AS " +
                                    "BEGIN " +
                                        "UPDATE dbo.Room " +
                                        "SET XPReward = ( " +
                                            "" +
                                            "SELECT MAX(Total_XP * Multiplier) " +
                                            "FROM (SELECT * FROM dbo.EncounterMultiplier) AS M " +
                                                "INNER JOIN ( " +
                                                    "SELECT SUM(XP*Count) AS Total_XP, SUM(Count) AS CountXP " +
                                                    "FROM ( " +
                                                        "( SELECT * " +
                                                            "FROM dbo.EnemyInRoom " +
                                                            "WHERE Room.RoomNumber = EnemyInRoom.RoomNumber " +
                                                                "AND Room.DungeonID = EnemyInRoom.DungeonID " +
                                                        ") AS EIR " +
                                                        "INNER JOIN " +
                                                        "dbo.Enemy AS E " +
                                                        "ON E.ID = EIR.EnemyID " +
                                                    ") " +
                                                ") AS XPs ON XPs.CountXP > M.CountMin " +
                                        "), " +
                                        "" +
                                        "XPRating = ( " +
                                            "SELECT SUM(XP*Count) AS Total_XP " +
                                            "FROM ( " +
                                                "( SELECT * " +
                                                    "FROM dbo.EnemyInRoom " +
                                                    "WHERE Room.RoomNumber = EnemyInRoom.RoomNumber " +
                                                        "AND Room.DungeonID = EnemyInRoom.DungeonID " +
                                                ") AS EIR " +
                                                "INNER JOIN " +
                                                "dbo.Enemy AS E " +
                                                "ON E.ID = EIR.EnemyID " +
                                        ") ), " +
                                        "" +
                                        "TotalValue = (" +
                                            "SELECT SUM(L.MonetaryValue * LIR.Count) " +
                                            "FROM ( ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.LootInRoom " +
                                                    "WHERE Room.RoomNumber = LootInRoom.RoomNumber " +
                                                        "AND Room.DungeonID = LootInRoom.DungeonID " +
                                                ") AS LIR " +
                                                "INNER JOIN " +
                                                "dbo.Loot AS L " +
                                                "ON L.ID = LIR.LootID " +
                                            ") " +
                                        "), " +
                                        "" +
                                        "TotalMoney = ( " +
                                            "SELECT SUM(L.MonetaryValue * LIR.Count) " +
                                            "FROM ( ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.LootInRoom " +
                                                    "WHERE Room.RoomNumber = LootInRoom.RoomNumber " +
                                                        "AND Room.DungeonID = LootInRoom.DungeonID " +
                                                ") AS LIR " +
                                                "INNER JOIN ( " +
                                                    "SELECT * " +
                                                    "FROM dbo.Loot " +
                                                    "WHERE IsMoney = 1 " +
                                                ") AS L " +
                                                "ON L.ID = LIR.LootID " +
                                            ") " +
                                        ") " +
                                        "" +
                                        "WHERE EXISTS ( " +
                                            "SELECT * " +
                                            "FROM Inserted " +
                                            "WHERE Inserted.RoomNumber = Room.RoomNumber " +
                                                "AND Inserted.DungeonID = Room.DungeonID " +
                                        ")" +
                                    " END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CRToXP");

            migrationBuilder.DropTable(
                name: "EncounterMultiplier");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "XPRating",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "XPReward",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "LootSet");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "LootSet");

            migrationBuilder.DropColumn(
                name: "XPRating",
                table: "EnemySet");

            migrationBuilder.DropColumn(
                name: "XPReward",
                table: "EnemySet");

            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_xp_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_set_xp_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_set_value_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.room_xp_value_compute");
        }
    }
}