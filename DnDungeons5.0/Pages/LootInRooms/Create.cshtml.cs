using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootInRooms
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
            // find all enemyIDs which are already associated with this room
            // can't use "await" and "ToListAsync()" cuz this function isn't Async
            IEnumerable<LootInRoom> lirs = _context.LootInRooms
                .Where(l => (l.RoomNum == roomNumber && l.DungeonID == dungeonID))
                .ToList();
            List<int> taken_loot_ids = new List<int>();
            foreach (var lir in lirs)
            {
                taken_loot_ids.Add(lir.LootID);
            }

            // filter list of potential enemyIDs accordingly
            ViewData["LootID"] = new SelectList(_context.Loots.Where(l => !taken_loot_ids.Contains(l.ID)), "ID", "Name");

            DungeonID = dungeonID;

            return Page();
        }

        [BindProperty]
        public LootInRoom LootInRoom { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            var emptyLIR = new LootInRoom();

            emptyLIR.RoomNum = roomNumber;
            emptyLIR.DungeonID = dungeonID;

            if (await TryUpdateModelAsync<LootInRoom>(
                emptyLIR,
                "lootinroom",   // Prefix for form value.
                d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
            {
                _context.LootInRooms.Add(emptyLIR);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }
    }
}
