using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyInRooms
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
            IEnumerable<EnemyInRoom> eirs = _context.EnemyInRooms
                .Where(e => (e.RoomNum == roomNumber && e.DungeonID == dungeonID))
                .ToList();
            List<int> taken_enemy_ids = new List<int>();
            foreach (var eir in eirs)
            {
                taken_enemy_ids.Add(eir.EnemyID);
            }

            // filter list of potential enemyIDs accordingly
            ViewData["EnemyID"] = new SelectList(_context.Enemies.Where(e => !taken_enemy_ids.Contains(e.ID)), "ID", "Name");

            DungeonID = dungeonID;

            return Page();
        }

        [BindProperty]
        public EnemyInRoom EnemyInRoom { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int roomNumber, int dungeonID)
        {
            var emptyEIR = new EnemyInRoom();

            emptyEIR.RoomNum = roomNumber;
            emptyEIR.DungeonID = dungeonID;

            if (await TryUpdateModelAsync<EnemyInRoom>(
                emptyEIR,
                "enemyinroom",   // Prefix for form value.
                d => d.EnemyID, d => d.Count, d => d.Name, d => d.Description))
            {
                _context.EnemyInRooms.Add(emptyEIR);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }

            return Page();
        }
    }
}
