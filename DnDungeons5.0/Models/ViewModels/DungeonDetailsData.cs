using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DnDungeons.Models.ViewModels
{
    public class DungeonDetailsData
    {
        public Dungeon Dungeon { get; set; }

        // [<Room, [<EIR, Enemy>], [<LIR, Loot>], [bool, RC, Room]>]
        public IEnumerable<Tuple<Room, IEnumerable<Tuple<EnemyInRoom, Enemy>>, IEnumerable<Tuple<LootInRoom, Loot>>, IEnumerable<Tuple<bool, RoomConnection, Room>>>> RoomInfo { get; set; }
    }
}
