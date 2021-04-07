using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class EnemyMedia
    {
        // key
        public int EnemyID { get; set; }
        public int MediaID { get; set; }

        // properties
        [StringLength(50, ErrorMessage = "Label cannot be longer than 50 characters.")]
        public string MediaLabel { get; set; }

        // navigation properties
        public Enemy Enemy { get; set; }
        public Media Media { get; set; }
    }
}
