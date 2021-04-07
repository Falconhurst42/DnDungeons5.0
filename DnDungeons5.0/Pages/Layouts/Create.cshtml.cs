using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Layouts
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
        public Layout Layout { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyLayout = new Layout();

            // emptyEnemy.Created = DateTime.UtcNow;

            if (await TryUpdateModelAsync<Layout>(
                emptyLayout,
                "layout",   // Prefix for form value.
                d => d.Name, d => d.Description))
            {
                _context.Layouts.Add(emptyLayout);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
