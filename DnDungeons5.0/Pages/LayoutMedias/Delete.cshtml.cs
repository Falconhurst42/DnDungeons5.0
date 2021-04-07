using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LayoutMedias
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LayoutMedia LayoutMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? layoutID, int? mediaID, bool? saveChangesError)
        {
            if (layoutID == null || mediaID == null)
            {
                return NotFound();
            }

            LayoutMedia = await _context.LayoutMedias
                .AsNoTracking().FirstOrDefaultAsync(m => (m.LayoutID == layoutID && m.MediaID == mediaID));

            if (LayoutMedia == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? layoutID, int? mediaID)
        {
            if (layoutID == null || mediaID == null)
            {
                return NotFound();
            }

            var lm = await _context.LayoutMedias.FindAsync(layoutID, mediaID);

            if (lm == null)
            {
                return NotFound();
            }
            try
            {
                _context.LayoutMedias.Remove(lm);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Layouts/Details", new { id = layoutID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { layoutID, mediaID, saveChangesError = true });
            }
        }
    }
}
