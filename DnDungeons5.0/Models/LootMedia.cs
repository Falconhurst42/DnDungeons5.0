using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class LootMedia
    {
        // key
        public int LootID { get; set; }
        public int MediaID { get; set; }

        // properties
        [StringLength(50, ErrorMessage = "Label cannot be longer than 50 characters.")]
        public string MediaLabel { get; set; }

        // navigation properties
        public Loot Loot { get; set; }
        public Media Media { get; set; }
    }
}
