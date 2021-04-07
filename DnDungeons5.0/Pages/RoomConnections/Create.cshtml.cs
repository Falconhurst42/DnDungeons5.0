using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.RoomConnections
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
            DungeonID = dungeonID;
        ViewData["Room2Num"] = new SelectList(_context.Rooms.Where(r => (r.DungeonID == dungeonID && r.RoomNumber != roomNumber)), "RoomNumber", "Name");
            return Page();
        }

        [BindProperty]
        public RoomConnection RoomConnection { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            var emptyRC = new RoomConnection();

            emptyRC.Room1Num = roomNumber;
            emptyRC.DungeonID = dungeonID;

            if (await TryUpdateModelAsync<RoomConnection>(
                emptyRC,
                "roomconnection",   // Prefix for form value.
                d => d.Room2Num, d => d.ConnectionPoint1, d => d.ConnectionPoint2))
            {
                _context.RoomConnections.Add(emptyRC);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }
    }
}
