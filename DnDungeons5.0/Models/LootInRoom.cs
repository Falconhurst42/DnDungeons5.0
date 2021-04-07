using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class LootInRoom
    {
        // key
        public int DungeonID { get; set; }
        public int RoomNum { get; set; }
        public int LootID { get; set; }

        // properties
        public int Count { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        // navigation
        public Room Room { get; set; }
        public Loot Loot { get; set; }
    }
}
