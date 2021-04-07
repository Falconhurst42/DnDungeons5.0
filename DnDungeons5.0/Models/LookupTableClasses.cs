using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DnDungeons.Models
{
    public class CRToXP
    {
        [Column(TypeName = "decimal(5, 3)")]
        public decimal CR { get; set; }
        public int XP { get; set; }
    }

    public class EncounterMultiplier
    {
        public int CountMin { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal Multiplier { get; set; }
    }
}
