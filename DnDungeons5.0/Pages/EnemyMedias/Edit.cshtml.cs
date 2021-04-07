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

namespace DnDungeons.Pages.EnemyMedias
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EnemyMedia EnemyMedia { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? enemyID, int? mediaID, bool? saveChangesError)
        {
            if (enemyID == null || mediaID == null)
            {
                return NotFound();
            }

            EnemyMedia = await _context.EnemyMedias.FindAsync(enemyID, mediaID);

            if (EnemyMedia == null)
            {
                return NotFound();
            }

            ICollection<EnemyMedia> ems = _context.EnemyMedias
               .Where(r => r.EnemyID == enemyID)
               .ToList();
            List<int> taken_media_ids = new List<int>();
            foreach (var em in ems)
            {
                taken_media_ids.Add(em.MediaID);
            }
            ViewData["MediaID"] = new SelectList(_context.Medias.Where(m => !taken_media_ids.Contains(m.ID)), "ID", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int enemyID, int mediaID)
        {
            var emToUpdate = await _context.EnemyMedias.FindAsync(enemyID, mediaID);

            if (emToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the MediaID (which is part of the key)
            // then we actually need to delete this EM and make a new one
            if (mediaID != EnemyMedia.MediaID)
            {
                // delete EM and make new one
                try
                {
                    _context.EnemyMedias.Remove(emToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyEM = new EnemyMedia();

                    emptyEM.EnemyID = enemyID;

                    if (await TryUpdateModelAsync<EnemyMedia>(
                        emptyEM,
                        "enemymedia",
                        d => d.MediaID, d => d.MediaLabel))
                    {
                        _context.EnemyMedias.Add(emptyEM);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Enemies/Details", new { id = enemyID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { enemyID, mediaID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<EnemyMedia>(
                emToUpdate,
                "enemymedia",
                d => d.MediaID, d => d.MediaLabel))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Enemies/Details", new { id = enemyID });
            }

            return Page();
        }

        private bool EnemyMediaExists(int id)
        {
            return _context.EnemyMedias.Any(e => e.EnemyID == id);
        }
    }
}
