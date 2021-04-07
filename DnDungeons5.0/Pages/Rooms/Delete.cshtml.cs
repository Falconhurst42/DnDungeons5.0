using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? roomNumber, int? dungeonID, bool? saveChangesError = false)
        {
            if (roomNumber == null || dungeonID == null)
            {
                return NotFound();
            }

            Room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(m => (m.RoomNumber == roomNumber && m.DungeonID == dungeonID));

            if (Room == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? roomNumber, int? dungeonID)
        {
            if (roomNumber == null || dungeonID == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(roomNumber, dungeonID);

            if (room == null)
            {
                return NotFound();
            }
            try
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { roomNumber, dungeonID, saveChangesError = true });
            }
        }
    }
}
