using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class EnemyInSet
    {
        // key
        public int EnemySetID { get; set; }
        public int EnemyID { get; set; }

        // properties
        public int Count { get; set; }
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        // navigation
        public EnemySet EnemySet { get; set; }
        public Enemy Enemy { get; set; }
    }
}
