using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.RoomConnections
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoomConnection RoomConnection { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? room1Number, int? room2Number, int? dungeonID, bool? saveChangesError)
        {
            if (room1Number == null || room2Number == null || dungeonID == null)
            {
                return NotFound();
            }

            // find the RC we want to edit
            RoomConnection = await _context.RoomConnections.FindAsync(dungeonID, room1Number, room2Number);

            if (RoomConnection == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? room1Number, int? room2Number, int? dungeonID)
        {
            if (room1Number == null || room2Number == null || dungeonID == null)
            {
                return NotFound();
            }

            var rc = await _context.RoomConnections.FindAsync(dungeonID, room1Number, room2Number);

            if (rc == null)
            {
                return NotFound();
            }
            try
            {
                _context.RoomConnections.Remove(rc);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { dungeonID, room1Number, room2Number, saveChangesError = true });
            }
        }
    }
}
