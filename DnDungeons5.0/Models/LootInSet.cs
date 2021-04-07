using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class LootInSet
    {
        // key
        public int LootSetID { get; set; }
        public int LootID { get; set; }

        // properties
        public int Count { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        // navigation
        public LootSet LootSet { get; set; }
        public Loot Loot { get; set; }
    }
}
