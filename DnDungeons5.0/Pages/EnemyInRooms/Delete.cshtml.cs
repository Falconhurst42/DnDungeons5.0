using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemyInRooms
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
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

            EnemyInRoom = await _context.EnemyInRooms
                .AsNoTracking()
                .FirstOrDefaultAsync(m => (m.RoomNum == roomNumber && m.DungeonID == dungeonID && m.EnemyID == enemyID));

            if (EnemyInRoom == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? roomNumber, int? dungeonID, int? enemyID)
        {
            if (roomNumber == null || dungeonID == null || enemyID == null)
            {
                return NotFound();
            }

            var eir = await _context.EnemyInRooms.FindAsync(dungeonID, roomNumber, enemyID);

            if (eir == null)
            {
                return NotFound();
            }
            try
            {
                _context.EnemyInRooms.Remove(eir);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { roomNumber, dungeonID, enemyID, saveChangesError = true });
            }
        }
    }
}
