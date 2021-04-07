using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LayoutMedias
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int LayoutID { get; set; }

        public IActionResult OnGet(int? layoutID)
        {
            ICollection<LayoutMedia> lms = _context.LayoutMedias
                .Where(r => r.LayoutID == layoutID)
                .ToList();
            List<int> taken_media_ids = new List<int>();
            foreach (var lm in lms)
            {
                taken_media_ids.Add(lm.MediaID);
            }
            ViewData["MediaID"] = new SelectList(_context.Medias.Where(m => !taken_media_ids.Contains(m.ID)), "ID", "Name");

            LayoutID = (int)layoutID;

            return Page();
        }

        [BindProperty]
        public LayoutMedia LayoutMedia { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int layoutID)
        {
            var emptyLM = new LayoutMedia();

            emptyLM.LayoutID = layoutID;

            if (await TryUpdateModelAsync<LayoutMedia>(
                emptyLM,
                "layoutmedia",   // Prefix for form value.
                d => d.MediaID, d => d.MediaLabel))
            {
                _context.LayoutMedias.Add(emptyLM);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Layouts/Details", new { id = layoutID });
            }

            return Page();
        }
    }
}
