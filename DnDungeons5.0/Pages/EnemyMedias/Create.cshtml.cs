using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyMedias
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int EnemyID { get; set; }

        public IActionResult OnGet(int? enemyID)
        {
            ICollection<EnemyMedia> ems = _context.EnemyMedias
                .Where(r => r.EnemyID == enemyID)
                .ToList();
            List<int> taken_media_ids = new List<int>();
            foreach (var em in ems)
            {
                taken_media_ids.Add(em.MediaID);
            }
            ViewData["MediaID"] = new SelectList(_context.Medias.Where(m => !taken_media_ids.Contains(m.ID)), "ID", "Name");

            EnemyID = (int)enemyID;

            return Page();
        }

        [BindProperty]
        public EnemyMedia EnemyMedia { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int enemyID)
        {
            var emptyEM = new EnemyMedia();

            emptyEM.EnemyID = enemyID;

            if (await TryUpdateModelAsync<EnemyMedia>(
                emptyEM,
                "enemymedia",   // Prefix for form value.
                d => d.MediaID, d => d.MediaLabel))
            {
                _context.EnemyMedias.Add(emptyEM);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Enemies/Details", new { id = enemyID });
            }

            return Page();
        }
    }
}
