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

namespace DnDungeons.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int? roomNumber, int? dungeonID)
        {
            if (roomNumber == null || dungeonID == null)
            {
                return NotFound();
            }

            Room = await _context.Rooms.FindAsync(roomNumber, dungeonID);

            if (Room == null)
            {
                return NotFound();
            }
           ViewData["DungeonID"] = new SelectList(_context.Dungeons, "ID", "Name");
           ViewData["LayoutID"] = new SelectList(_context.Layouts, "ID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            var roomToUpdate = await _context.Rooms.FindAsync(roomNumber, dungeonID);

            if (roomToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Room>(
                roomToUpdate,
                "room",
                d => d.Name, d => d.Description, d => d.LayoutID))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomNumber == id);
        }
    }
}
