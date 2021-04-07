using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootMedias
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LootMedia LootMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? lootID, int? mediaID, bool? saveChangesError)
        {
            if (lootID == null || mediaID == null)
            {
                return NotFound();
            }

            LootMedia = await _context.LootMedias
                .AsNoTracking().FirstOrDefaultAsync(m => (m.LootID == lootID && m.MediaID == mediaID));

            if (LootMedia == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? lootID, int? mediaID)
        {
            if (lootID == null || mediaID == null)
            {
                return NotFound();
            }

            var lm = await _context.LootMedias.FindAsync(lootID, mediaID);

            if (lm == null)
            {
                return NotFound();
            }
            try
            {
                _context.LootMedias.Remove(lm);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Loots/Details", new { id = lootID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { lootID, mediaID, saveChangesError = true });
            }
        }
    }
}
