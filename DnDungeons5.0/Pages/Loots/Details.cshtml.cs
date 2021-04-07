using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Loots
{
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public Loot Loot { get; set; }

        public ICollection<LootMedia> LootMedias { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Loot = await _context.Loots.FirstOrDefaultAsync(m => m.ID == id);

            if (Loot == null)
            {
                return NotFound();
            }

            // load related Media links and Media
            // not sure why this didn't work for Dungeons, but this is a more intended manner of loading related data
            LootMedias = await _context.LootMedias
                .Where(em => em.LootID == id)
                .Include(em => em.Media)
                .ToListAsync();

            return Page();
        }
    }
}
