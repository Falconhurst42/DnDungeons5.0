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
    public class AddEnemySetModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public AddEnemySetModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public int DungeonID { get; set; }
        public ICollection<EnemySet> EnemySets { get; set; }
        public string AddedMessage { get; set; }

        public IActionResult OnGet(int? roomNumber, int? dungeonID, string addedMessage = "")
        {
            if(roomNumber == null || dungeonID == null )
            {
                return NotFound();
            }

            EnemySets = _context.EnemySets
                .Include(es => es.EnemiesInSet)
                .ThenInclude(eis => eis.Enemy)
                .ToList();

            DungeonID = (int)dungeonID;
            AddedMessage = addedMessage;

            return Page();
        }

        [BindProperty]
        public int EnemySetID { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            AddedMessage = "";

            // get enemy ids which are already associated with this room
            var taken_enemy_ids = await _context.EnemyInRooms
                .Where(eir => (eir.DungeonID == dungeonID && eir.RoomNum == roomNumber))
                .Select(eir => eir.EnemyID)
                .ToListAsync();

            // get enemyInSets which are associated with the chosen set but not with an enemy in the previous list
            var eiss = await _context.EnemyInSets
                .Where(eis => (eis.EnemySetID == EnemySetID && !taken_enemy_ids.Contains(eis.EnemyID)))
                .Include(eis => eis.Enemy)
                .ToListAsync();

            // add set
                // foreach eis
                // create a copy eir

            // for each enemy in the set
            foreach(EnemyInSet eis in eiss)
            {
                // make a new EIR
                EnemyInRoom emptyEIR = new EnemyInRoom();

                // link EIR to proper room
                emptyEIR.DungeonID = dungeonID;
                emptyEIR.RoomNum = roomNumber;

                // copy data from eis
                emptyEIR.EnemyID = eis.EnemyID;
                emptyEIR.Count = eis.Count;
                emptyEIR.Name = eis.Name;
                emptyEIR.Description = eis.Description;

                // add EIR to database
                _context.EnemyInRooms.Add(emptyEIR);
                await _context.SaveChangesAsync();

                AddedMessage += $"Added {(eis.Name == null ? eis.Enemy.Name : eis.Name)} x{eis.Count}\n";
            }
            // remove trailing newline
            AddedMessage.Trim('\n');

            // return to adding page
            return RedirectToAction("./AddEnemySet",
                                         new { roomNumber, dungeonID, saveChangesError = true });
        }
    }
}
