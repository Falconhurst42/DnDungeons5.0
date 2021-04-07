using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class FinalCleanup2TriggerBoogaloo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dropping triggers which were loooping on eachother and combing them into single triggers

        // Drop triggers
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_xp_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_set_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_set_xp_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_set_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_set_value_compute");
            migrationBuilder.Sql("DROP TRIGGER dbo.room_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.room_xp_value_compute");

        // combine triggers
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_xp_lu_update ON dbo.Enemy " +
                                    "AFTER INSERT, UPDATE " +
                                    "AS " +
                                    "BEGIN " +
                                        "UPDATE dbo.Enemy " +
                                        "SET Enemy.XP = (" +
                                            "SELECT XP " +
                                            "FROM dbo.CRToXP " +
                                            "WHERE CRToXP.CR=Enemy.CR" +
                                        "), " +
                                        "LastUpdated = GETUTCDATE() " +
                                        "WHERE Enemy.ID IN (SELECT DISTINCT ID FROM Inserted) " +
                                    "END");

            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_set_xp_lu_update ON dbo.EnemySet " +
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
                                        ") ), " +
                                        "LastUpdated = GETUTCDATE() " +
                                        "" +
                                        "WHERE EnemySet.ID IN(SELECT DISTINCT ID FROM Inserted) " +
                                        "" +
                                    "END");

            migrationBuilder.Sql("CREATE TRIGGER dbo.loot_set_value_lu_update ON dbo.LootSet " +
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
                                        "), " +
                                        "LastUpdated = GETUTCDATE()" +
                                        "" +
                                        "WHERE LootSet.ID IN(SELECT DISTINCT ID FROM Inserted) " +
                                    "END");

            migrationBuilder.Sql("CREATE TRIGGER dbo.room_xp_value_lu_update ON dbo.Room " +
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
                                        "), " +
                                        "LastUpdated = GETUTCDATE()" +
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
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_xp_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_set_xp_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_set_value_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.room_xp_value_lu_update");

            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_lu_update ON dbo.Enemy AFTER UPDATE AS BEGIN UPDATE dbo.Enemy SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_set_lu_update ON dbo.EnemySet AFTER UPDATE AS BEGIN UPDATE dbo.EnemySet SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.loot_set_lu_update ON dbo.LootSet AFTER UPDATE AS BEGIN UPDATE dbo.LootSet SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_lu_update ON dbo.Room AFTER UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE EXISTS (SELECT * FROM Inserted WHERE Inserted.RoomNumber=RoomNumber AND Inserted.DungeonID=DungeonID) END");

            
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
    }
}
