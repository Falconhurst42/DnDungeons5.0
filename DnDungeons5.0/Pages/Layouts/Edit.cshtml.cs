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

namespace DnDungeons.Pages.Layouts
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Layout Layout { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Layout = await _context.Layouts.FindAsync(id);

            if (Layout == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var layoutToUpdate = await _context.Layouts.FindAsync(id);

            if (layoutToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Layout>(
                layoutToUpdate,
                "layout",
                d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }

        private bool LayoutExists(int id)
        {
            return _context.Layouts.Any(e => e.ID == id);
        }
    }
}
