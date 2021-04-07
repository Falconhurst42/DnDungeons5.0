using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Rooms
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int DungeonID { get; set; }

        public IActionResult OnGet(int roomNumber, int dungeonID)
        {
        ViewData["DungeonID"] = new SelectList(_context.Dungeons, "ID", "Name");
        ViewData["LayoutID"] = new SelectList(_context.Layouts, "ID", "Name");

            DungeonID = dungeonID;

            return Page();
        }

        [BindProperty]
        public Room Room { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            var emptyRoom = new Room();

            emptyRoom.RoomNumber = roomNumber;
            emptyRoom.DungeonID = dungeonID;

            if (await TryUpdateModelAsync<Room>(
                emptyRoom,
                "room",   // Prefix for form value.
                d => d.Name, d => d.Description, d => d.LayoutID))
            {
                _context.Rooms.Add(emptyRoom);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }
    }
}
