using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnDungeons.Models
{
    public class RoomConnection
    {
        // key
        [Display(Name = "Dungeon ID")]
        public int DungeonID { get; set; }
        [Display(Name = "Room #1")]
        public int Room1Num { get; set; }
        [Display(Name = "Room #2")]
        public int Room2Num { get; set; }

        // properties
        [StringLength(50)]
        [Display(Name = "Room 1 Connection Point")]
        public string ConnectionPoint1 { get; set; }
        [StringLength(50)]
        [Display(Name = "Room 1 Connection Point")]
        public string ConnectionPoint2 { get; set; }

        // navigation
        public Dungeon Dungeon { get; set; }
        public Room Room1 { get; set; }
        public Room Room2 { get; set; }
    }
}
