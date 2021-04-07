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

namespace DnDungeons.Pages.LootMedias
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LootMedia LootMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? lootID, int? mediaID, bool? saveChangesError)
        {
            if (lootID == null || mediaID == null)
            {
                return NotFound();
            }

            LootMedia = await _context.LootMedias.FindAsync(lootID, mediaID);

            if (LootMedia == null)
            {
                return NotFound();
            }

            ICollection<LootMedia> lms = _context.LootMedias
               .Where(r => r.LootID == lootID)
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
        public async Task<IActionResult> OnPostAsync(int lootID, int mediaID)
        {
            var lmToUpdate = await _context.LootMedias.FindAsync(lootID, mediaID);

            if (lmToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the MediaID (which is part of the key)
            // then we actually need to delete this EM and make a new one
            if (mediaID != LootMedia.MediaID)
            {
                // delete EM and make new one
                try
                {
                    _context.LootMedias.Remove(lmToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyLM = new LootMedia();

                    emptyLM.LootID = lootID;

                    if (await TryUpdateModelAsync<LootMedia>(
                        emptyLM,
                        "lootmedia",
                        d => d.MediaID, d => d.MediaLabel))
                    {
                        _context.LootMedias.Add(emptyLM);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Loots/Details", new { id = lootID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { lootID, mediaID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<LootMedia>(
                lmToUpdate,
                "lootmedia",
                d => d.MediaID, d => d.MediaLabel))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Loots/Details", new { id = lootID });
            }

            return Page();
        }

        private bool LootMediaExists(int id)
        {
            return _context.LootMedias.Any(e => e.LootID == id);
        }
    }
}
