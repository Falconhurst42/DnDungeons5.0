using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemySets
{
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public EnemySet EnemySet { get; set; }

        public ICollection<EnemyInSet> EnemyInSets { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EnemySet = await _context.EnemySets.FindAsync(id);

            if (EnemySet == null)
            {
                return NotFound();
            }

            // load related EnemyInSets and Enemies
            // not sure why this didn't work for Dungeons, but this is a more intended manner of loading related data
            EnemyInSets = await _context.EnemyInSets
                .Where(em => em.EnemySetID == id)
                .Include(em => em.Enemy)
                .ToListAsync();

            return Page();
        }
    }
}
