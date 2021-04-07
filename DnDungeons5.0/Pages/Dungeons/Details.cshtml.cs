using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Models;
using DnDungeons.Models.ViewModels;
using DnDungeons.Data;

namespace DnDungeons.Pages.Dungeons
{
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public DungeonDetailsData DungeonDetails { get; set; }
        public int NextRoomNumber { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Initialize data structure
            DungeonDetails = new DungeonDetailsData();
            
            // get dungeon and rooms with LINQ
            DungeonDetails.Dungeon = await _context.Dungeons.FindAsync(id);
            IEnumerable<Room> Rooms = await _context.Rooms
                .Where(r => (r.DungeonID == id))
                .ToListAsync();

            // Loading room data using a series of loops and sql queries
            // (for the record, I shouldn't have to do this, but loading the objects the normal way refuses to work for me
            // so I've resorted to using the most explicit method I can think of)
            // what should've worked:
            /* Dungeon = await _context.Dungeons
               .Include(d => d.Rooms)
                   .ThenInclude(r => r.Enemies)
                       .ThenInclude(el => el.Enemy)
               .Include(d => d.Rooms)
                   .ThenInclude(r => r.Loot)
                       .ThenInclude(ll => ll.Loot)
               .Include(d => d.Rooms)
                   .ThenInclude(r => r.Layout)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.ID == id); */
            // See for more details on the intended method: https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-5.0

            // This data structure consists of a list of rooms which are paired with info about their corresponding enemies and loot
             // More specifically, I'm denoting the datatype as: [<Room, [<EnemyInRoom, Enemy>], [<LootInRoom, Loot>], [<bool, RoomConnection, Room>] where [] denotes IEnum and <> denotes tuple
             //                                                   ^Room^ ^ List of Enemy info ^  ^List of Loot info ^  ^Is_1st^      ^RC^    ^other R^
            DungeonDetails.RoomInfo = Enumerable.Empty<Tuple<Room, IEnumerable<Tuple<EnemyInRoom, Enemy>>, IEnumerable<Tuple<LootInRoom, Loot>>, IEnumerable<Tuple<bool, RoomConnection, Room>>>>();

            // for each Room
            foreach (Room room in Rooms)
            {
                // Make Enemy list
                    // make a temporary list of all the EIRs associated with this room each paired with the associated Enemy
                    IEnumerable<Tuple<EnemyInRoom, Enemy>> e_temp_tup_enum = Enumerable.Empty<Tuple<EnemyInRoom, Enemy>>();

                    // get all associated EnemyInRoom linkers into a container
                     // using a SQL query on the EIR table filtering by dungID and Room#
                    IEnumerable<EnemyInRoom> eirs = _context.EnemyInRooms
                        .FromSqlRaw("SELECT * FROM dbo.EnemyInRoom")
                        .Where(e => (e.DungeonID == id && e.RoomNum == room.RoomNumber));

                    // for each EIR
                    foreach (EnemyInRoom eir in eirs)
                    {
                        // get the associated Enemy
                         // using a SQL query on the Enemy table filtering by ID
                        Enemy e = _context.Enemies
                            .FromSqlRaw("SELECT * FROM dbo.Enemy")
                            .Where(_e => (_e.ID == eir.EnemyID))
                            .Single();

                        // combine the EIR and the Enemy into a tuple and add it to the list for this Room
                        e_temp_tup_enum = e_temp_tup_enum.Append(Tuple.Create(eir, e));
                    }

                // Make Loot list
                    // make a temporary list of all the LIRs associated with this room each paired with the associated Loot
                    IEnumerable<Tuple<LootInRoom, Loot>> l_temp_tup_enum = Enumerable.Empty<Tuple<LootInRoom, Loot>>();

                    // get all associated LootInRoom linkers into a container
                     // using a SQL query on the LIR table filtering by dungID and Room#
                    IEnumerable<LootInRoom> lirs = await _context.LootInRooms
                        .Where(l => (l.DungeonID == id && l.RoomNum == room.RoomNumber))
                        .ToListAsync();

                    // for each LIR
                    foreach (LootInRoom lir in lirs)
                    {
                        // get the associated Loot
                         // using a SQL query on the Loot table filtering by ID
                        Loot l = await _context.Loots
                            .Where(_e => (_e.ID == lir.LootID))
                            .SingleAsync();

                        // combine the LIR and the Loot into a tuple and add it to the list for this Room
                        l_temp_tup_enum = l_temp_tup_enum.Append(Tuple.Create(lir, l));
                    }

                // make RoomConnection List
                IEnumerable<RoomConnection> rc_temp_enum = await _context.RoomConnections
                    .Where(rc => (rc.DungeonID == id && (rc.Room1Num == room.RoomNumber || rc.Room2Num == room.RoomNumber)))
                    .ToListAsync();

                IEnumerable<Tuple<bool, RoomConnection, Room>> rc_temp_tup_enum = Enumerable.Empty<Tuple<bool, RoomConnection, Room>>();
                foreach (RoomConnection rc in rc_temp_enum)
                {
                    bool is_first = rc.Room1Num == room.RoomNumber;
                    // check if the other room actually exists
                    try
                    {
                        // don't bother checking if the other roomNum is -1 (signals that the other room was deleted)
                        if ((is_first ? rc.Room2Num : rc.Room1Num) != -1)
                        {
                            Room other = await _context.Rooms
                                .Where(r => (r.DungeonID == id && r.RoomNumber == (is_first ? rc.Room2Num : rc.Room1Num)))
                                .SingleAsync();
                            rc_temp_tup_enum = rc_temp_tup_enum.Append(Tuple.Create(is_first, rc, other));
                        }
                    }
                    catch { }
                }

                // make a tuple out of the current room and the corresponding Enemy, Loot, and RoomConnection lists
                // append that tuple to the IEnum
                DungeonDetails.RoomInfo = DungeonDetails.RoomInfo.Append(Tuple.Create(room, e_temp_tup_enum, l_temp_tup_enum, rc_temp_tup_enum));
            }

            // propogate RoomConnections

            // Get the next available room number
            // probably could write an SQL query for this, but it's not a big performance concern and I can't be arsed (and I'm not 100% sure how the LINQ query system works)
            List<int> taken_nums = new List<int>();
            foreach (var room_info in DungeonDetails.RoomInfo)
            {
                taken_nums.Add(room_info.Item1.RoomNumber);
            }
            NextRoomNumber = 1;
            while(taken_nums.Contains(NextRoomNumber)) {
                NextRoomNumber++;
            }            

            return Page();
        }
    }
}

// correlated lists
/*DungeonDetails = new DungeonDetailsData();
DungeonDetails.Rooms = await _context.Rooms
        .Include(r => r.Enemies)
            .ThenInclude(el => el.Enemy)
        .Include(r => r.Loot)
            .ThenInclude(ll => ll.Loot)
        .Include(r => r.Layout)
    .AsNoTracking()
    .ToListAsync();

if(id != null)
{
    foreach (Room room in DungeonDetails.Rooms)
    {
        DungeonDetails.EIRs.Append(room.Enemies);
        DungeonDetails.Enemies.Append(room.Enemies.Select(el => el.Enemy));
        DungeonDetails.LIRs.Append(room.Loot);
        DungeonDetails.Loot.Append(room.Loot.Select(el => el.Loot));
    }
}*/

// Dungeon list
/*IEnumerable<Dungeon> DS = await _context.Dungeons
    .AsSplitQuery()
    .Include(d => d.Rooms)
        .ThenInclude(r => r.Enemies)
            .ThenInclude(el => el.Enemy)
    .Include(d => d.Rooms)
        .ThenInclude(r => r.Loot)
            .ThenInclude(ll => ll.Loot)
    .Include(d => d.Rooms)
        .ThenInclude(r => r.Layout)
    .AsNoTracking()
    .ToListAsync();*/

// Explicit load?
/*Dungeon = await _context.Dungeons
    .Include(d => d.Rooms)
    .AsNoTracking()
    .FirstOrDefaultAsync(m => m.ID == id);*/

/*_context.Rooms.Where(r => r.DungeonID == id).Load();
foreach (Room room in _context.Rooms)
{
    _context.EnemyInRooms.Where(el => (el.RoomNum == room.RoomNumber && el.DungeonID == room.DungeonID)).Load();
    foreach(EnemyInRoom el in _context.EnemyInRooms)
    {
        _context.Enemies.Where(e => (e.ID == el.EnemyID)).Load();
    }
}*/
