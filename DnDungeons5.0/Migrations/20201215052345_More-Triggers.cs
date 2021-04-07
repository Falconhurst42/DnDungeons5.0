using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class MoreTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // updated LastUpdated attribute on update
            // adapted query from: https://stackoverflow.com/a/7737993
            // This answer pointed me in the right direction when it WHERE-IN clauses with multiple columns: https://stackoverflow.com/a/29479935
            // but SQL server doesn't support that syntax, so I had to find a new way
            // eventually, I found this answer which inspired me to use the EXISTS syntax to solve my problem: https://stackoverflow.com/a/58945038
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_lu_update ON dbo.Enemy AFTER UPDATE AS BEGIN UPDATE dbo.Enemy SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.loot_lu_update ON dbo.Loot AFTER UPDATE AS BEGIN UPDATE dbo.Loot SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.layout_lu_update ON dbo.Layout AFTER UPDATE AS BEGIN UPDATE dbo.Layout SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.enemy_set_lu_update ON dbo.EnemySet AFTER UPDATE AS BEGIN UPDATE dbo.EnemySet SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.loot_set_lu_update ON dbo.LootSet AFTER UPDATE AS BEGIN UPDATE dbo.LootSet SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.media_lu_update ON dbo.Media AFTER UPDATE AS BEGIN UPDATE dbo.Media SET LastUpdated = GETUTCDATE() WHERE ID IN(SELECT DISTINCT ID FROM Inserted) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_lu_update ON dbo.Room AFTER UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE EXISTS (SELECT * FROM Inserted WHERE Inserted.RoomNumber=RoomNumber AND Inserted.DungeonID=DungeonID) END");

            // When Room deleted set porper RoomNum attribute of corresponding RoomConnection to -1
            // delete RoomConnections where both RoomNums are -1
            // just kinda winged it tbh
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_rc_cascade_delete ON dbo.Room AFTER DELETE AS BEGIN UPDATE dbo.RoomConnection SET Room1Num = -1 WHERE ((DungeonID IN (SELECT DISTINCT DungeonID FROM DELETED)) AND(Room1Num IN (SELECT DISTINCT RoomNumber FROM Deleted))) UPDATE dbo.RoomConnection SET Room2Num = -1 WHERE ((DungeonID IN (SELECT DISTINCT DungeonID FROM DELETED)) AND (Room2Num IN(SELECT DISTINCT RoomNumber FROM Deleted))) DELETE FROM dbo.RoomConnection WHERE (Room1Num = -1 AND Room2Num = -1) END");

            // When Room deleted, delete all corresponding EIR/LIR
            // again using the EXISTS approach from https://stackoverflow.com/a/58945038
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_eir_lir_cascade_delete ON dbo.Room AFTER DELETE AS BEGIN DELETE FROM dbo.EnemyInRoom WHERE EXISTS (SELECT * FROM Deleted WHERE Deleted.RoomNumber=RoomNum AND Deleted.DungeonID=DungeonID) DELETE FROM dbo.LootInRoom WHERE EXISTS (SELECT * FROM Deleted WHERE Deleted.RoomNumber=RoomNum AND Deleted.DungeonID=DungeonID) END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.layout_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.enemy_set_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.loot_set_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.media_lu_update");
            migrationBuilder.Sql("DROP TRIGGER dbo.room_lu_update");

            migrationBuilder.Sql("DROP TRIGGER dbo.room_cs_cascase_delete");

            migrationBuilder.Sql("DROP TRIGGER dbo.room_eir_lir_cascade_delete");
        }
    }
}
