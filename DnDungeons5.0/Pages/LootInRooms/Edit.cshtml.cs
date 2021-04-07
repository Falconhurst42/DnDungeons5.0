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

namespace DnDungeons.Pages.LootInRooms
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LootInRoom LootInRoom { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? roomNumber, int? dungeonID, int? lootID, bool? saveChangesError)
        {
            if (roomNumber == null || dungeonID == null || lootID == null)
            {
                return NotFound();
            }

            // find the EIR we want to edit
            LootInRoom = await _context.LootInRooms.FindAsync(dungeonID, roomNumber, lootID);

            if (LootInRoom == null)
            {
                return NotFound();
            }

            // find all enemyIDs which are already associated with this room (NOT including current enemyID)
            IEnumerable<LootInRoom> lirs = await _context.LootInRooms
                .Where(l => (l.RoomNum == roomNumber && l.DungeonID == dungeonID))
                .ToListAsync();
            List<int> taken_loot_ids = new List<int>();
            //taken_enemy_ids.Add((int)enemyID);
            foreach (var lir in lirs)
            {
                taken_loot_ids.Add(lir.LootID);
            }

            // filter list of potential enemyIDs accordingly
            // do not exclude the current enemy
            // select the current enemy
            ViewData["LootID"] = new SelectList(_context.Loots.Where(l => (l.ID == lootID || !taken_loot_ids.Contains(l.ID))), "ID", "Name", lootID);

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID, int lootID)
        {
            var lirToUpdate = await _context.LootInRooms.FindAsync(dungeonID, roomNumber, lootID);

            if (lirToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the lootID (which is part of the key)
            // then we actually need to delete this LIR and make a new one
            if (lootID != LootInRoom.LootID)
            {
                // delete EIR
                try
                {
                    _context.LootInRooms.Remove(lirToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyLIR = new LootInRoom();

                    emptyLIR.DungeonID = dungeonID;
                    emptyLIR.RoomNum = roomNumber;

                    if (await TryUpdateModelAsync<LootInRoom>(
                        emptyLIR,
                        "lootinroom",
                        d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
                    {
                        _context.LootInRooms.Add(emptyLIR);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { roomNumber, dungeonID, lootID, saveChangesError = true });
                }
            }

            if (await TryUpdateModelAsync<LootInRoom>(
                lirToUpdate,
                "lootinroom",
                d => d.LootID, d => d.Count, d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }

        private bool LootInRoomExists(int id)
        {
            return _context.LootInRooms.Any(e => e.DungeonID == id);
        }
    }
}
