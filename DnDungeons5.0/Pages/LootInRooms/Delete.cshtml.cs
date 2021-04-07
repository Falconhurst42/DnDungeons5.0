using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootInRooms
{
    public class DeleteModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DeleteModel(DnDungeons.Data.DnDungeonsContext context)
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

            LootInRoom = await _context.LootInRooms
                .AsNoTracking()
                .FirstOrDefaultAsync(m => (m.RoomNum == roomNumber && m.DungeonID == dungeonID && m.LootID == lootID));

            if (LootInRoom == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? roomNumber, int? dungeonID, int? lootID)
        {
            if (roomNumber == null || dungeonID == null || lootID == null)
            {
                return NotFound();
            }

            var lir = await _context.LootInRooms.FindAsync(dungeonID, roomNumber, lootID);

            if (lir == null)
            {
                return NotFound();
            }
            try
            {
                _context.LootInRooms.Remove(lir);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                                     new { roomNumber, dungeonID, lootID, saveChangesError = true });
            }
        }
    }
}
