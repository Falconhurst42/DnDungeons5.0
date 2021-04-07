using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Models;
using DnDungeons.Data;

namespace DnDungeons.Pages.Dungeons
{
    public class IndexModel : PageModel
    {
        // searching, filtering, and pagination from https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/sort-filter-page?view=aspnetcore-5.0

        private readonly DnDungeonsContext _context;

        public IndexModel(DnDungeonsContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateCreatedSort { get; set; }
        public string DateUpdatedSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Dungeon> Dungeons { get;set; }

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

            IQueryable<Dungeon> dungeonsIQ = from d in _context.Dungeons
                                             select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                dungeonsIQ = dungeonsIQ.Where(d => d.Name.Contains(searchString)
                                       || d.Description.Contains(searchString));
            }

            dungeonsIQ = sortOrder switch
            {
                "name" => dungeonsIQ.OrderBy(d => d.Name),
                "name_desc" => dungeonsIQ.OrderByDescending(d => d.Name),
                "date_u_old" => dungeonsIQ.OrderBy(d => d.LastUpdated),
                "date_c" => dungeonsIQ.OrderByDescending(d => d.Created),
                "date_c_old" => dungeonsIQ.OrderBy(d => d.Created),
                _ => dungeonsIQ.OrderByDescending(d => d.LastUpdated),
            };
            int pageSize = 7;
            Dungeons = await PaginatedList<Dungeon>.CreateAsync(
                dungeonsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
