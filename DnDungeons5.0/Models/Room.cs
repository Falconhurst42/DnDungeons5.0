using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnDungeons.Models
{
    public class Room
    {
        // key
        [Display(Name = "Room #")]
        public int RoomNumber { get; set; }
        [Display(Name = "Dungeon ID")]
        public int DungeonID { get; set; }

        // properties
        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Date Created")]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes/databasegenerated-attribute
        public DateTime Created { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}")]
        [Display(Name = "Last Updated")]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes/databasegenerated-attribute
        public DateTime LastUpdated { get; set; }

        // navigation
        public int? LayoutID { get; set; }

        public Dungeon Dungeon { get; set; }
        public Layout Layout { get; set; }
        public ICollection<EnemyInRoom> Enemies { get; set; }
        public ICollection<LootInRoom> Loot { get; set; }
        // public ICollection<RoomConnection> Connections { get; set; }

        // computed
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalValue { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalMoney { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? XPRating { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? XPReward { get; set; }
        // https://docs.microsoft.com/en-us/ef/core/saving/explicit-values-generated-properties
        // https://docs.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations#value-generated-on-add-or-update
    }
}
