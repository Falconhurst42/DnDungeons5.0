using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootMedias
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int LootID { get; set; }

        public IActionResult OnGet(int? lootID)
        {
            ICollection<LootMedia> lms = _context.LootMedias
                .Where(r => r.LootID == lootID)
                .ToList();
            List<int> taken_media_ids = new List<int>();
            foreach (var lm in lms)
            {
                taken_media_ids.Add(lm.MediaID);
            }
            ViewData["MediaID"] = new SelectList(_context.Medias.Where(m => !taken_media_ids.Contains(m.ID)), "ID", "Name");

            LootID = (int)lootID;

            return Page();
        }

        [BindProperty]
        public LootMedia LootMedia { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int lootID)
        {
            var emptyLM = new LootMedia();

            emptyLM.LootID = lootID;

            if (await TryUpdateModelAsync<LootMedia>(
                emptyLM,
                "lootmedia",   // Prefix for form value.
                d => d.MediaID, d => d.MediaLabel))
            {
                _context.LootMedias.Add(emptyLM);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Loots/Details", new { id = lootID });
            }

            return Page();
        }
    }
}
