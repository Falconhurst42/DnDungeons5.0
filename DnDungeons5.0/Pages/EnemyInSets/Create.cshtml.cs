using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyInSets
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int EnemySetID { get; set; }

        public IActionResult OnGet(int enemySetID)
        {
            // find all enemyIDs which are already associated with this set
            // can't use "await" and "ToListAsync()" cuz this function isn't Async
            IEnumerable<EnemyInSet> eiss = _context.EnemyInSets
                .Where(e => e.EnemySetID == enemySetID)
                .ToList();
            List<int> taken_enemy_ids = new List<int>();
            foreach (var eis in eiss)
            {
                taken_enemy_ids.Add(eis.EnemyID);
            }

            // filter list of potential enemyIDs accordingly
            ViewData["EnemyID"] = new SelectList(_context.Enemies.Where(e => !taken_enemy_ids.Contains(e.ID)), "ID", "Name");

            EnemySetID = enemySetID;

            return Page();
        }

        [BindProperty]
        public EnemyInSet EnemyInSet { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int enemySetID)
        {
            var emptyEIS = new EnemyInSet();

            emptyEIS.EnemySetID = enemySetID;

            if (await TryUpdateModelAsync<EnemyInSet>(
                emptyEIS,
                "enemyinset",   // Prefix for form value.
                d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
            {
                _context.EnemyInSets.Add(emptyEIS);
                await _context.SaveChangesAsync();
                return RedirectToPage("/EnemySets/Details", new { id = enemySetID });
            }

            return Page();
        }
    }
}
