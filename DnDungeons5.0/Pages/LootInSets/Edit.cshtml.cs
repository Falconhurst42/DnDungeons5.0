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

namespace DnDungeons.Pages.LootInSets
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LootInSet LootInSet { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? lootSetID, int? lootID, bool? saveChangesError)
        {
            if (lootSetID == null || lootID == null)
            {
                return NotFound();
            }

            LootInSet = await _context.LootInSets.FindAsync(lootSetID, lootID);

            if (LootInSet == null)
            {
                return NotFound();
            }

            // find all enemyIDs which are already associated with this set
            IEnumerable<LootInSet> liss = await _context.LootInSets
                .Where(l => l.LootSetID == lootSetID)
                .ToListAsync();
            List<int> taken_loot_ids = new List<int>();
            foreach (var lis in liss)
            {
                taken_loot_ids.Add(lis.LootID);
            }

            // filter list of potential enemyIDs accordingly
            // do not exclude the current enemy
            // select the current enemy
            ViewData["LootID"] = new SelectList(_context.Loots.Where(l => (l.ID == lootID || !taken_loot_ids.Contains(l.ID))), "ID", "Name", lootID);

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int lootSetID, int lootID)
        {
            var lisToUpdate = await _context.LootInSets.FindAsync(lootSetID, lootID);

            if (lisToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the lootID (which is part of the key)
            // then we actually need to delete this LIS and make a new one
            if (lootID != LootInSet.LootID)
            {
                // delete LIS and make new one
                try
                {
                    _context.LootInSets.Remove(lisToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyLIS = new LootInSet();

                    emptyLIS.LootSetID = lootSetID;

                    if (await TryUpdateModelAsync<LootInSet>(
                        emptyLIS,
                        "lootinset",
                        d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
                    {
                        _context.LootInSets.Add(emptyLIS);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/LootSets/Details", new { id = lootSetID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { lootSetID, lootID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<LootInSet>(
                lisToUpdate,
                "lootinset",
                d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/LootSets/Details", new { id = lootSetID });
            }

            return Page();
        }

        private bool LootInSetExists(int id)
        {
            return _context.LootInSets.Any(e => e.LootSetID == id);
        }
    }
}
