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

namespace DnDungeons.Pages.Enemies
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Enemy Enemy { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enemy = await _context.Enemies.FindAsync(id);

            if (Enemy == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var enemyToUpdate = await _context.Enemies.FindAsync(id);

            if (enemyToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Enemy>(
                enemyToUpdate,
                "enemy",
                d => d.Name, d => d.Description, d => d.XP))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool EnemyExists(int id)
        {
            return _context.Enemies.Any(e => e.ID == id);
        }
    }
}
