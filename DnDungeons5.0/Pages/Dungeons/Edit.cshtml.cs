using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Models;
using DnDungeons.Data;

namespace DnDungeons.Pages.Dungeons
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Dungeon Dungeon { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dungeon = await _context.Dungeons.FindAsync(id);

            if (Dungeon == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var dungeonToUpdate = await _context.Dungeons.FindAsync(id);

            if (dungeonToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Dungeon>(
                dungeonToUpdate,
                "dungeon",
                d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool DungeonExists(int id)
        {
            return _context.Dungeons.Any(e => e.ID == id);
        }
    }
}
