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

namespace DnDungeons.Pages.LayoutMedias
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LayoutMedia LayoutMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? layoutID, int? mediaID, bool? saveChangesError)
        {
            if (layoutID == null || mediaID == null)
            {
                return NotFound();
            }

            LayoutMedia = await _context.LayoutMedias.FindAsync(layoutID, mediaID);

            if (LayoutMedia == null)
            {
                return NotFound();
            }

            ICollection<LayoutMedia> lms = _context.LayoutMedias
               .Where(r => r.LayoutID == layoutID)
               .ToList();
            List<int> taken_media_ids = new List<int>();
            foreach (var lm in lms)
            {
                taken_media_ids.Add(lm.MediaID);
            }
            ViewData["MediaID"] = new SelectList(_context.Medias.Where(m => !taken_media_ids.Contains(m.ID)), "ID", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int layoutID, int mediaID)
        {
            var lmToUpdate = await _context.LayoutMedias.FindAsync(layoutID, mediaID);

            if (lmToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the MediaID (which is part of the key)
            // then we actually need to delete this EM and make a new one
            if (mediaID != LayoutMedia.MediaID)
            {
                // delete EM and make new one
                try
                {
                    _context.LayoutMedias.Remove(lmToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyLM = new LayoutMedia();

                    emptyLM.LayoutID = layoutID;

                    if (await TryUpdateModelAsync<LayoutMedia>(
                        emptyLM,
                        "layoutmedia",
                        d => d.MediaID, d => d.MediaLabel))
                    {
                        _context.LayoutMedias.Add(emptyLM);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Layouts/Details", new { id = layoutID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { layoutID, mediaID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<LayoutMedia>(
                lmToUpdate,
                "layoutmedia",
                d => d.MediaID, d => d.MediaLabel))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Layouts/Details", new { id = layoutID });
            }

            return Page();
        }

        private bool LayoutExists(int id)
        {
            return _context.Layouts.Any(e => e.ID == id);
        }
    }
}
