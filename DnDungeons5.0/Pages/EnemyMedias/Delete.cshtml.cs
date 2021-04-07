using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyMedias
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EnemyMedia EnemyMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? enemyID, int? mediaID, bool? saveChangesError)
        {
            if (enemyID == null || mediaID == null)
            {
                return NotFound();
            }

            EnemyMedia = await _context.EnemyMedias
                .AsNoTracking().FirstOrDefaultAsync(m => (m.EnemyID == enemyID && m.MediaID == mediaID));

            if (EnemyMedia == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? enemyID, int? mediaID)
        {
            if (enemyID == null || mediaID == null)
            {
                return NotFound();
            }

            var em = await _context.EnemyMedias.FindAsync(enemyID, mediaID);

            if (em == null)
            {
                return NotFound();
            }
            try
            {
                _context.EnemyMedias.Remove(em);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Enemies/Details", new { id = enemyID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { enemyID, mediaID, saveChangesError = true });
            }
        }
    }
}
