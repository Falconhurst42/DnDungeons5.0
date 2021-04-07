using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootInSets
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
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

            LootInSet = await _context.LootInSets
                .AsNoTracking().FirstOrDefaultAsync(m => (m.LootSetID == lootSetID && m.LootID == lootID));

            if (LootInSet == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? lootSetID, int? lootID)
        {
            if (lootSetID == null || lootID == null)
            {
                return NotFound();
            }

            var lis = await _context.LootInSets.FindAsync(lootSetID, lootID);

            if (lis == null)
            {
                return NotFound();
            }
            try
            {
                _context.LootInSets.Remove(lis);
                await _context.SaveChangesAsync();
                return RedirectToPage("/LootSets/Details", new { id = lootSetID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { lootSetID, lootID, saveChangesError = true });
            }
        }
    }
}
