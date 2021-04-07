using Microsoft.EntityFrameworkCore.Migrations;

namespace DnDungeons.Migrations
{
    public partial class LUCascadeTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // adapted queries from: https://stackoverflow.com/a/7737993
            // multi-column where-in replacement couresty of https://stackoverflow.com/a/58945038

            // update last-updated of Dungeon when one of its Rooms is created/editted/deleted
            migrationBuilder.Sql("CREATE TRIGGER dbo.room_lu_cascade ON dbo.Room AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.Dungeon SET LastUpdated = GETUTCDATE() WHERE (Dungeon.ID IN(SELECT DISTINCT Inserted.DungeonID FROM Inserted)) OR (Dungeon.ID IN(SELECT DISTINCT Deleted.DungeonID FROM Deleted)) END");

            // update last-updated of Room when one of its EIRs/LIR/RCs is created/editted/deleted
            migrationBuilder.Sql("CREATE TRIGGER dbo.eir_lu_cascade ON dbo.EnemyInRoom AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE (EXISTS(SELECT * FROM Inserted WHERE Inserted.RoomNum = Room.RoomNumber AND Inserted.DungeonID = Room.DungeonID) OR EXISTS(SELECT * FROM Deleted WHERE Deleted.RoomNum = Room.RoomNumber AND Deleted.DungeonID = Room.DungeonID)) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.lir_lu_cascade ON dbo.LootInRoom AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE (EXISTS(SELECT * FROM Inserted WHERE Inserted.RoomNum = Room.RoomNumber AND Inserted.DungeonID = Room.DungeonID) OR EXISTS(SELECT * FROM Deleted WHERE Deleted.RoomNum = Room.RoomNumber AND Deleted.DungeonID = Room.DungeonID)) END");
            migrationBuilder.Sql("CREATE TRIGGER dbo.rc_lu_cascade ON dbo.RoomConnection AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE (EXISTS(SELECT * FROM Inserted WHERE ((Inserted.Room1Num = Room.RoomNumber OR Inserted.Room2Num = Room.RoomNumber) AND Inserted.DungeonID = Room.DungeonID)) OR EXISTS(SELECT * FROM Deleted WHERE ((Deleted.Room1Num = Room.RoomNumber OR Deleted.Room2Num = Room.RoomNumber) AND Deleted.DungeonID = Room.DungeonID))) END");

            // update last-updated of EnemySet when one of its EISs is created/editted/deleted
            migrationBuilder.Sql("CREATE TRIGGER dbo.eis_lu_cascade ON dbo.EnemyInSet AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.EnemySet SET LastUpdated = GETUTCDATE() WHERE ((ID IN(SELECT DISTINCT EnemySetID FROM Inserted)) OR (ID IN(SELECT DISTINCT EnemySetID FROM Deleted))) END");

            // update last-updated of LootSet when one of its LISs is created/editted/deleted
            migrationBuilder.Sql("CREATE TRIGGER dbo.lis_lu_cascade ON dbo.LootInSet AFTER DELETE, INSERT, UPDATE AS BEGIN UPDATE dbo.LootSet SET LastUpdated = GETUTCDATE() WHERE ((ID IN(SELECT DISTINCT LootSetID FROM Inserted)) OR (ID IN(SELECT DISTINCT LootSetID FROM Deleted))) END");


            // change the trigger that handles roomconnections when a corresponding room is deleted
            // just deletes connection instead of doing the whole -1 thing
            migrationBuilder.Sql("ALTER TRIGGER dbo.room_rc_cascade_delete ON dbo.Room AFTER DELETE AS BEGIN DELETE FROM dbo.RoomConnection WHERE ((DungeonID IN (SELECT DISTINCT DungeonID FROM DELETED)) AND ((Room1Num IN (SELECT DISTINCT RoomNumber FROM Deleted)) OR (Room1Num IN (SELECT DISTINCT RoomNumber FROM Deleted)))) END");

            // fix the trigger that handles room_lu updating (was updating the LU of every room)
            migrationBuilder.Sql("ALTER TRIGGER dbo.room_lu_update ON dbo.Room AFTER UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE EXISTS (SELECT * FROM Inserted WHERE Inserted.RoomNumber=Room.RoomNumber AND Inserted.DungeonID=Room.DungeonID) END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // drop new triggers
            migrationBuilder.Sql("DROP TRIGGER dbo.room_lu_cascade");
            migrationBuilder.Sql("DROP TRIGGER dbo.eir_lu_cascade");
            migrationBuilder.Sql("DROP TRIGGER dbo.lir_lu_cascade");
            migrationBuilder.Sql("DROP TRIGGER dbo.rc_lu_cascade");
            migrationBuilder.Sql("DROP TRIGGER dbo.eis_lu_cascade");
            migrationBuilder.Sql("DROP TRIGGER dbo.lis_lu_cascade");

            // revert cahnge to room-roomconnection and room_lu triggers
            migrationBuilder.Sql("ALTER TRIGGER dbo.room_eir_lir_cascade_delete ON dbo.Room AFTER DELETE AS BEGIN DELETE FROM dbo.EnemyInRoom WHERE EXISTS (SELECT * FROM Deleted WHERE Deleted.RoomNumber=RoomNum AND Deleted.DungeonID=DungeonID) DELETE FROM dbo.LootInRoom WHERE EXISTS (SELECT * FROM Deleted WHERE Deleted.RoomNumber=RoomNum AND Deleted.DungeonID=DungeonID) END");
            migrationBuilder.Sql("ALTER TRIGGER dbo.room_lu_update ON dbo.Room AFTER UPDATE AS BEGIN UPDATE dbo.Room SET LastUpdated = GETUTCDATE() WHERE EXISTS (SELECT * FROM Inserted WHERE Inserted.RoomNumber=RoomNumber AND Inserted.DungeonID=DungeonID) END");
        }
    }
}
