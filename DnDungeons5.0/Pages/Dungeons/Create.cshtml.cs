using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Models;
using DnDungeons.Data;

namespace DnDungeons.Pages.Dungeons
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Dungeon Dungeon { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyDungeon = new Dungeon();

            // emptyDungeon.Created = DateTime.UtcNow;

            if (await TryUpdateModelAsync<Dungeon>(
                emptyDungeon,
                "dungeon",   // Prefix for form value.
                d => d.Name, d => d.Description))
            {
                _context.Dungeons.Add(emptyDungeon);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
