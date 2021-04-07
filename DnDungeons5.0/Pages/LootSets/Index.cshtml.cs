using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.LootSets
{
    public class IndexModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public IndexModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateCreatedSort { get; set; }
        public string DateUpdatedSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<LootSet> LootSets { get;set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            DateUpdatedSort = String.IsNullOrEmpty(sortOrder) ? "date_u_old" : "";
            NameSort = (sortOrder == "name") ? "name_desc" : "name";
            DateCreatedSort = (sortOrder == "date_c") ? "date_c_old" : "date_c";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<LootSet> lootSetsIQ = from ls in _context.LootSets
                                             select ls;

            if (!String.IsNullOrEmpty(searchString))
            {
                lootSetsIQ = lootSetsIQ.Where(ls => ls.Name.Contains(searchString)
                                       || ls.Description.Contains(searchString));
            }

            lootSetsIQ = sortOrder switch
            {
                "name" => lootSetsIQ.OrderBy(ls => ls.Name),
                "name_desc" => lootSetsIQ.OrderByDescending(ls => ls.Name),
                "date_u_old" => lootSetsIQ.OrderBy(d => d.LastUpdated),
                "date_c" => lootSetsIQ.OrderByDescending(d => d.Created),
                "date_c_old" => lootSetsIQ.OrderBy(d => d.Created),
                _ => lootSetsIQ.OrderByDescending(ls => ls.LastUpdated),
            };
            int pageSize = 7;
            LootSets = await PaginatedList<LootSet>.CreateAsync(
                lootSetsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
