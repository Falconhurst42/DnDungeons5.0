using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemySets
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
        public EnemySet EnemySet { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyEnemySet = new EnemySet();

            if (await TryUpdateModelAsync<EnemySet>(
                emptyEnemySet,
                "enemyset",   // Prefix for form value.
                d => d.Name, d => d.Description))
            {
                _context.EnemySets.Add(emptyEnemySet);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
