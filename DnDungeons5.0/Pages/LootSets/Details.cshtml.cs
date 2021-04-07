using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootSets
{
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public LootSet LootSet { get; set; }

        public ICollection<LootInSet> LootInSets { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LootSet = await _context.LootSets.FindAsync(id);

            if (LootSet == null)
            {
                return NotFound();
            }

            // load related LootInSets and Loot
            // not sure why this didn't work for Dungeons, but this is a more intended manner of loading related data
            LootInSets = await _context.LootInSets
                .Where(em => em.LootSetID == id)
                .Include(em => em.Loot)
                .ToListAsync();

            return Page();
        }
    }
}
