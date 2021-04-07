using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyInSets
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EnemyInSet EnemyInSet { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? enemySetID, int? enemyID, bool? saveChangesError)
        {
            if (enemySetID == null || enemyID == null)
            {
                return NotFound();
            }

            EnemyInSet = await _context.EnemyInSets.FindAsync(enemySetID, enemyID);

            if (EnemyInSet == null)
            {
                return NotFound();
            }

            // find all enemyIDs which are already associated with this set
            IEnumerable<EnemyInSet> eiss = await _context.EnemyInSets
                .Where(e => e.EnemySetID == enemySetID)
                .ToListAsync();
            List<int> taken_enemy_ids = new List<int>();
            foreach (var eis in eiss)
            {
                taken_enemy_ids.Add(eis.EnemyID);
            }

            // filter list of potential enemyIDs accordingly
            // do not exclude the current enemy
            // select the current enemy
            ViewData["EnemyID"] = new SelectList(_context.Enemies.Where(e => (e.ID == enemyID || !taken_enemy_ids.Contains(e.ID))), "ID", "Name", enemyID);

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int enemySetID, int enemyID)
        {
            var eisToUpdate = await _context.EnemyInSets.FindAsync(enemySetID, enemyID);

            if (eisToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the enemyID (which is part of the key)
            // then we actually need to delete this EIS and make a new one
            if (enemyID != EnemyInSet.EnemyID)
            {
                // delete EIS and make new one
                try
                {
                    _context.EnemyInSets.Remove(eisToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyEIS = new EnemyInSet();

                    emptyEIS.EnemySetID = enemySetID;

                    if (await TryUpdateModelAsync<EnemyInSet>(
                        emptyEIS,
                        "enemyinset",
                        d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
                    {
                        _context.EnemyInSets.Add(emptyEIS);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/EnemySets/Details", new { id = enemySetID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { enemySetID, enemyID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<EnemyInSet>(
                eisToUpdate,
                "enemyinset",
                d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/EnemySets/Details", new { id = enemySetID });
            }

            return Page();
        }

        private bool EnemyInSetExists(int id)
        {
            return _context.EnemyInSets.Any(e => e.EnemySetID == id);
        }
    }
}
