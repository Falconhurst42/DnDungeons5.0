using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Rooms
{
    public class AddLootSetModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public AddLootSetModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int DungeonID { get; set; }
        public ICollection<LootSet> LootSets { get; set; }
        public string AddedMessage { get; set; }

        public IActionResult OnGet(int? roomNumber, int? dungeonID, string addedMessage = "")
        {
            if (roomNumber == null || dungeonID == null)
            {
                return NotFound();
            }

            LootSets = _context.LootSets
                .Include(es => es.LootInSet)
                .ThenInclude(lis => lis.Loot)
                .ToList();

            DungeonID = (int)dungeonID;
            AddedMessage = addedMessage;

            return Page();
        }

        [BindProperty]
        public int LootSetID { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            AddedMessage = "";

            // get enemy ids which are already associated with this room
            var taken_enemy_ids = await _context.LootInRooms
                .Where(lir => (lir.DungeonID == dungeonID && lir.RoomNum == roomNumber))
                .Select(lir => lir.LootID)
                .ToListAsync();

            // get enemyInSets which are associated with the chosen set but not with an enemy in the previous list
            var liss = await _context.LootInSets
                .Where(lis => (lis.LootSetID == LootSetID && !taken_enemy_ids.Contains(lis.LootID)))
                .Include(lis => lis.Loot)
                .ToListAsync();

            // add set
            // foreach lis
            // create a copy lir

            // for each enemy in the set
            foreach (LootInSet lis in liss)
            {
                // make a new LIR
                LootInRoom emptyLIR = new LootInRoom();

                // link LIR to proper room
                emptyLIR.DungeonID = dungeonID;
                emptyLIR.RoomNum = roomNumber;

                // copy data from lis
                emptyLIR.LootID = lis.LootID;
                emptyLIR.Count = lis.Count;
                emptyLIR.Name = lis.Name;
                emptyLIR.Description = lis.Description;

                // add LIR to database
                _context.LootInRooms.Add(emptyLIR);
                await _context.SaveChangesAsync();

                AddedMessage += $"Added {(lis.Name == null ? lis.Loot.Name : lis.Name)} x{lis.Count}\n";
            }
            // remove trailing newline
            AddedMessage.Trim('\n');

            // return to adding page
            return RedirectToAction("./AddLootSet",
                                         new { roomNumber, dungeonID, addedMessage = AddedMessage });
        }
    }
}
