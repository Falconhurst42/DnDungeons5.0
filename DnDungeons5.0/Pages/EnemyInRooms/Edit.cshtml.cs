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

namespace DnDungeons.Pages.EnemyInRooms
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EnemyInRoom EnemyInRoom { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? roomNumber, int? dungeonID, int? enemyID, bool? saveChangesError)
        {
            if (roomNumber == null || dungeonID == null || enemyID == null)
            {
                return NotFound();
            }

            // find the EIR we want to edit
            EnemyInRoom = await _context.EnemyInRooms.FindAsync(dungeonID, roomNumber, enemyID);

            if (EnemyInRoom == null)
            {
                return NotFound();
            }

            // find all enemyIDs which are already associated with this room
            IEnumerable<EnemyInRoom> eirs = await _context.EnemyInRooms
                .Where(e => (e.RoomNum == roomNumber && e.DungeonID == dungeonID))
                .ToListAsync();
            List<int> taken_enemy_ids = new List<int>();
            //taken_enemy_ids.Add((int)enemyID);
            foreach(var eir in eirs)
            {
                taken_enemy_ids.Add(eir.EnemyID);
            }
            
            // filter list of potential enemyIDs accordingly
            // do not exclude the current enemy
            // select the current enemy
            ViewData["EnemyID"] = new SelectList(_context.Enemies.Where(e => (e.ID==enemyID || !taken_enemy_ids.Contains(e.ID))), "ID", "Name", enemyID);

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID, int enemyID)
        {
            var eirToUpdate = await _context.EnemyInRooms.FindAsync(dungeonID, roomNumber, enemyID);

            if (eirToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed the enemyID (which is part of the key)
            // then we actually need to delete this EIR and make a new one
            if (enemyID != EnemyInRoom.EnemyID)
            {
                // delete EIR and make new one
                try
                {
                    _context.EnemyInRooms.Remove(eirToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyEIR = new EnemyInRoom();

                    emptyEIR.DungeonID = dungeonID;
                    emptyEIR.RoomNum = roomNumber;

                    if (await TryUpdateModelAsync<EnemyInRoom>(
                        emptyEIR,
                        "enemyinroom",
                        d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
                    {
                        _context.EnemyInRooms.Add(emptyEIR);
                        await _context.SaveChangesAsync();
                        return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                    }

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { roomNumber, dungeonID, enemyID, saveChangesError = true });
                }
            }

            // otherwise just update
            if (await TryUpdateModelAsync<EnemyInRoom>(
                eirToUpdate,
                "enemyinroom",
                d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }

        private bool EnemyInRoomExists(int id)
        {
            return _context.EnemyInRooms.Any(e => e.DungeonID == id);
        }
    }
}
