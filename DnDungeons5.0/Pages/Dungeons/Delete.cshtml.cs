using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Models;
using DnDungeons.Data;

namespace DnDungeons.Pages.Dungeons
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Dungeon Dungeon { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dungeon = await _context.Dungeons
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Dungeon == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dungeon = await _context.Dungeons.FindAsync(id);

            if (dungeon == null)
            {
                return NotFound();
            }
            try
            {
                _context.Dungeons.Remove(dungeon);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { id, saveChangesError = true });
            }
        }
    }
}
