using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Enemies
{
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public Enemy Enemy { get; set; }

        public ICollection<EnemyMedia> EnemyMedias { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enemy = await _context.Enemies.FindAsync(id);

            if (Enemy == null)
            {
                return NotFound();
            }

            // load related Media links and Media
            // not sure why this didn't work for Dungeons, but this is a more intended manner of loading related data
            EnemyMedias = await _context.EnemyMedias
                .Where(em => em.EnemyID == id)
                .Include(em => em.Media)
                .ToListAsync();            

            return Page();
        }
    }
}
