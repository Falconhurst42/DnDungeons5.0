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

namespace DnDungeons.Pages.RoomConnections
{
    public class EditModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public EditModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoomConnection RoomConnection { get; set; }
        public bool IsFirst { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? room1Number, int? room2Number, int? dungeonID, bool? isFirst, bool? saveChangesError)
        {
            IsFirst = (bool)isFirst;

            if (room1Number == null || room2Number == null || dungeonID == null || isFirst == null)
            {
                return NotFound();
            }

            // find the RC we want to edit
            RoomConnection = await _context.RoomConnections.FindAsync(dungeonID, room1Number, room2Number);

            if (RoomConnection == null)
            {
                return NotFound();
            }

            // get list of possible room connections
            if((bool)isFirst)
            {
                ViewData["Room2Num"] = new SelectList(_context.Rooms.Where(r => (r.DungeonID==dungeonID && r.RoomNumber!=room1Number)), "RoomNumber", "Name", room2Number);
            }
            else
            {
                ViewData["Room1Num"] = new SelectList(_context.Rooms.Where(r => (r.DungeonID == dungeonID && r.RoomNumber != room2Number)), "RoomNumber", "Name", room1Number);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int room1Number, int room2Number, int dungeonID, bool isFirst)
        {
            var rcToUpdate = await _context.RoomConnections.FindAsync(dungeonID, room1Number, room2Number);

            if (rcToUpdate == null)
            {
                return NotFound();
            }

            // if we've changed either roomNumber (which is part of the key)
            // then we actually need to delete this RC and make a new one
            if (room1Number != RoomConnection.Room1Num || room2Number != RoomConnection.Room2Num)
            {
                // delete RC and make new one
                try
                {
                    _context.RoomConnections.Remove(rcToUpdate);
                    await _context.SaveChangesAsync();

                    var emptyRC = new RoomConnection();

                    emptyRC.DungeonID = dungeonID;
                    if(isFirst)
                    {
                        emptyRC.Room1Num = room1Number;

                        if (await TryUpdateModelAsync<RoomConnection>(
                        emptyRC,
                        "roomconnection",
                        d => d.Room2Num, d => d.ConnectionPoint1, d => d.ConnectionPoint2))
                        {
                            _context.RoomConnections.Add(emptyRC);
                            await _context.SaveChangesAsync();
                            return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                        }
                    }
                    else
                    {
                        emptyRC.Room2Num = room2Number;

                        if (await TryUpdateModelAsync<RoomConnection>(
                        emptyRC,
                        "roomconnection",
                        d => d.Room1Num, d => d.ConnectionPoint1, d => d.ConnectionPoint2))
                        {
                            _context.RoomConnections.Add(emptyRC);
                            await _context.SaveChangesAsync();
                            return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                        }
                    }

                    

                    return Page();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Edit",
                                         new { room1Number, room2Number, dungeonID, isFirst, saveChangesError = true });
                }
            }

            // otherwise just update
            if (isFirst)
            {
                if (await TryUpdateModelAsync<RoomConnection>(
                rcToUpdate,
                "roomconnection",
                d => d.Room2Num, d => d.ConnectionPoint1, d => d.ConnectionPoint2))
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                }
            }
            else
            {
                if (await TryUpdateModelAsync<RoomConnection>(
                rcToUpdate,
                "roomconnection",
                d => d.Room1Num, d => d.ConnectionPoint1, d => d.ConnectionPoint2))
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Dungeons/Details", new { id = dungeonID });
                }
            }

            return Page();
        }

        private bool RoomConnectionExists(int id)
        {
            return _context.RoomConnections.Any(e => e.DungeonID == id);
        }
    }
}
