using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyInSets
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
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

            EnemyInSet = await _context.EnemyInSets
                .AsNoTracking().FirstOrDefaultAsync(m => (m.EnemySetID == enemySetID && m.EnemyID == enemyID));

            if (EnemyInSet == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? enemySetID, int? enemyID)
        {
            if (enemySetID == null || enemyID == null)
            {
                return NotFound();
            }

            var eis = await _context.EnemyInSets.FindAsync(enemySetID, enemyID);

            if (eis == null)
            {
                return NotFound();
            }
            try
            {
                _context.EnemyInSets.Remove(eis);
                await _context.SaveChangesAsync();
                return RedirectToPage("/EnemySets/Details", new { id = enemySetID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { enemySetID, enemyID, saveChangesError = true });
            }
        }
    }
}
