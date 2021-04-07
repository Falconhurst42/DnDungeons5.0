using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnDungeons.Models
{
    public enum EnemyType
    {
        Hostile, Friendly, Other
    }
    public class Enemy
    {
        // key = ID
        public int ID { get; set; }

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
        public ICollection<EnemyMedia> AssociatedMedia { get; set; }

        // calculated
        [Required]
        [Column(TypeName = "decimal(5, 3)")] // https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/da1?view=aspnetcore-5.0#update-the-generated-code
        public decimal CR { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? XP { get; set; }
    }
}
