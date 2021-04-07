using DnDungeons.Data;
using DnDungeons.Models;
using System;
using System.Linq;

namespace DnDungeons.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DnDungeonsContext context)
        {
            // context.Database.EnsureCreated();

            // Look for any dungeons.
            if (context.Dungeons.Any())
            {
                return;   // DB has been seeded
            }

            /*var medias = new Media[]
            {
                new Media{Name="dummy",Description="placeholder", FileName="test", FileType=".png"}
            };

            context.Medias.AddRange(medias);
            context.SaveChanges();*/

            var layouts = new Layout[]
            {
                new Layout{Name="Big Room 1",Description="Big room, nice pillars."},
                new Layout{Name="Coatroom",Description="Rows and rows of coats."},
                new Layout{Name="Guardsroom",Description="Weapons and benches."},
                new Layout{Name="Cellar",Description="Foodstuffs and such."},
                new Layout{Name="Big Room 2",Description="Big room, nice tapestries."},
                new Layout{Name="Dungheap",Description="Ew."}
            };

            context.Layouts.AddRange(layouts);
            context.SaveChanges();

            var loots = new Loot[]
            {
                new Loot{Name="Platinum Piece",Description="More money than most peasants see in their entire life.", IsMoney=true, MonetaryValue=1000},
                new Loot{Name="Copper Piece",Description="Penny analog, but actually worth something.", IsMoney=true, MonetaryValue=1},
                new Loot{Name="Bottle of Wine",Description="A nice Cabernet.", IsMoney=false, MonetaryValue=200},
                new Loot{Name="Poop",Description="Why is this even loot?", IsMoney=false}
            };

            context.Loots.AddRange(loots);
            context.SaveChanges();

            var enemies = new Enemy[]
            {
                new Enemy{Name="Skeleton",Description="Animated bones.", CR=0.25M, XP=50},
                new Enemy{Name="Zombie",Description="Animated corpse.", CR=0.25M, XP=50},
                new Enemy{Name="Lion",Description="A whole-ass lion.", CR=1M, XP=200},
                new Enemy{Name="Bad Mage",Description="Does bad magic.", CR=1M, XP=200},
                new Enemy{Name="Dullahan",Description="The black horse strides out of the shadows with its nostrils huffing steam. Its rider, swathed in black leather, raises his arm to reveal not a lantern but its own severed, grinning head.", CR=11M, XP=7200}
            };

            context.Enemies.AddRange(enemies);
            context.SaveChanges();

            var dungeons = new Dungeon[]
            {
                new Dungeon{Name="Dullahan Dungheap",Description="It stinky. It deadly"},
                new Dungeon{Name="Place of the Bad Man", Description="Very Bad Man make place, but treasure!"}
            };

            context.Dungeons.AddRange(dungeons);
            context.SaveChanges();

            var rooms = new Room[]
            {
                new Room{Name="Evil Entryhall",Description="Impressive mural, bad vibe.", RoomNumber=1,
                        LayoutID=layouts.Single(l => l.Name=="Big Room 1").ID, DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID},
                new Room{Name="Surprisingly Big Coatroom",Description="What are a lion and a witch doing in a wardrobe?", RoomNumber=2,
                        LayoutID=layouts.Single(l => l.Name=="Coatroom").ID, DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID},
                new Room{Name="Undead Guardsroom",Description="Those skeletons look angry.", RoomNumber=3,
                        LayoutID=layouts.Single(l => l.Name=="Guardsroom").ID, DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID},
                new Room{Name="Musty Cellar",Description="Bad air, but some nice vintages.", RoomNumber=4,
                        LayoutID=layouts.Single(l => l.Name=="Cellar").ID, DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID},
                new Room{Name="Throne Room",Description="Big room, nice tapestries, pity about the corpses.", RoomNumber=5,
                        LayoutID=layouts.Single(l => l.Name=="Big Room 2").ID, DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID},
                new Room{Name="Dungheap",Description="Ew.", RoomNumber=1,
                        LayoutID=layouts.Single(l => l.Name=="Dungheap").ID, DungeonID=dungeons.Single(d => d.Name=="Dullahan Dungheap").ID}
            };

            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            var lootinrooms = new LootInRoom[]
            {
                new LootInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    LootID=loots.Single(l => l.Name=="Bottle of Wine").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Musty Cellar")).RoomNumber,
                    Name="Botle of Fine Wine", Description="Some nice Cabernets.", Count=5},
                new LootInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    LootID=loots.Single(l => l.Name=="Copper Piece").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Surprisingly Big Coatroom")).RoomNumber,
                    Description="There's some loose change hidden in the coats.", Count=69},
                new LootInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    LootID=loots.Single(l => l.Name=="Platinum Piece").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Throne Room")).RoomNumber,
                    Description="On the arm of the throne is a stack of coin, mere pocket change to the Bad Man.", Count=8},
                new LootInRoom{DungeonID=dungeons.Single(d => d.Name=="Dullahan Dungheap").ID,
                    LootID=loots.Single(l => l.Name=="Poop").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Dullahan Dungheap").ID && r.Name=="Dungheap")).RoomNumber,
                    Description="There's a lot of it.", Count=100}
            };

            context.LootInRooms.AddRange(lootinrooms);
            context.SaveChanges();

            var enemyinrooms = new EnemyInRoom[]
            {
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    EnemyID=enemies.Single(l => l.Name=="Skeleton").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Undead Guardsroom")).RoomNumber,
                    Description="Reanimated bones bound to the defense of this fel place.", Count=5},
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    EnemyID=enemies.Single(l => l.Name=="Lion").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Surprisingly Big Coatroom")).RoomNumber,
                    Name="Nalsa", Description="A massive lion with a flowing mane.", Count=1},
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    EnemyID=enemies.Single(l => l.Name=="Bad Mage").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Surprisingly Big Coatroom")).RoomNumber,
                    Name="White Witch", Description="Basic af.", Count=1},
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    EnemyID=enemies.Single(l => l.Name=="Zombie").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Throne Room")).RoomNumber,
                    Name="Fallen Adventurer", Description="The reanimated corpses of those who came before.", Count=5},
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    EnemyID=enemies.Single(l => l.Name=="Bad Mage").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Throne Room")).RoomNumber,
                    Name="The Bad Man", Description="A gaunt figure with glowing eyes clad in tattered finery.", Count=1},
                new EnemyInRoom{DungeonID=dungeons.Single(d => d.Name=="Dullahan Dungheap").ID,
                    EnemyID=enemies.Single(l => l.Name=="Dullahan").ID,
                    RoomNum=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Dullahan Dungheap").ID && r.Name=="Dungheap")).RoomNumber,
                    Name="Celty Sturluson", Description="What's a Dullahan doing here?", Count=1}
            };

            context.EnemyInRooms.AddRange(enemyinrooms);
            context.SaveChanges();

            var roomconnections = new RoomConnection[]
            {
                new RoomConnection{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    Room1Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Evil Entryhall")).RoomNumber,
                    Room2Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Undead Guardsroom")).RoomNumber,
                    ConnectionPoint1="Northeast door", ConnectionPoint2="South door"},
                new RoomConnection{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    Room1Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Evil Entryhall")).RoomNumber,
                    Room2Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Surprisingly Big Coatroom")).RoomNumber,
                    ConnectionPoint1="Northwest door", ConnectionPoint2="South door"},
                new RoomConnection{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    Room1Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Surprisingly Big Coatroom")).RoomNumber,
                    Room2Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Musty Cellar")).RoomNumber,
                    ConnectionPoint1="East door", ConnectionPoint2="West door"},
                new RoomConnection{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    Room1Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Musty Cellar")).RoomNumber,
                    Room2Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Undead Guardsroom")).RoomNumber,
                    ConnectionPoint1="East door", ConnectionPoint2="West door"},
                new RoomConnection{DungeonID=dungeons.Single(d => d.Name=="Place of the Bad Man").ID,
                    Room1Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Undead Guardsroom")).RoomNumber,
                    Room2Num=rooms.Single(r => (r.DungeonID==dungeons.Single(d => d.Name=="Place of the Bad Man").ID && r.Name=="Throne Room")).RoomNumber,
                    ConnectionPoint1="North door", ConnectionPoint2="South door"}
            };

            context.RoomConnections.AddRange(roomconnections);
            context.SaveChanges();
        }
    }
}