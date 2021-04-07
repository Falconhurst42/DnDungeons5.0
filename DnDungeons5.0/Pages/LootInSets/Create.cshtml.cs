using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootInSets
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int LootSetID { get; set; }

        public IActionResult OnGet(int lootSetID)
        {
            // find all LootIDs which are already associated with this set
            // can't use "await" and "ToListAsync()" cuz this function isn't Async
            IEnumerable<LootInSet> liss = _context.LootInSets
                .Where(l => l.LootSetID == lootSetID)
                .ToList();
            List<int> taken_loot_ids = new List<int>();
            foreach (var lis in liss)
            {
                taken_loot_ids.Add(lis.LootID);
            }

            // filter list of potential LootIDs accordingly
            ViewData["LootID"] = new SelectList(_context.Loots.Where(l => !taken_loot_ids.Contains(l.ID)), "ID", "Name");

            LootSetID = lootSetID;

            return Page();
        }

        [BindProperty]
        public LootInSet LootInSet { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int lootSetID)
        {
            var emptyLIS = new LootInSet();

            emptyLIS.LootSetID = lootSetID;

            if (await TryUpdateModelAsync<LootInSet>(
                emptyLIS,
                "lootinset",   // Prefix for form value.
                d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
            {
                _context.LootInSets.Add(emptyLIS);
                await _context.SaveChangesAsync();
                return RedirectToPage("/LootSets/Details", new { id = lootSetID });
            }

            return Page();
        }
    }
}
