using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Enemies
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
        public string DifficultySort { get; set; }

        public PaginatedList<Enemy> Enemies { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            DateUpdatedSort = String.IsNullOrEmpty(sortOrder) ? "date_u_old" : "";
            NameSort = (sortOrder == "name") ? "name_desc" : "name";
            DateCreatedSort = (sortOrder == "date_c") ? "date_c_old" : "date_c";
            DifficultySort = (sortOrder == "difficulty") ? "difficulty_desc" : "difficulty";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Enemy> enemiesIQ = from l in _context.Enemies
                                       select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                enemiesIQ = enemiesIQ.Where(e => e.Name.Contains(searchString)
                                       || e.Description.Contains(searchString));
            }

            enemiesIQ = sortOrder switch
            {
                "name" => enemiesIQ.OrderBy(d => d.Name),
                "name_desc" => enemiesIQ.OrderByDescending(d => d.Name),
                "date_u_old" => enemiesIQ.OrderBy(d => d.LastUpdated),
                "date_c" => enemiesIQ.OrderByDescending(d => d.Created),
                "date_c_old" => enemiesIQ.OrderBy(d => d.Created),
                "difficulty" => enemiesIQ.OrderBy(d => d.CR).ThenBy(d => d.XP),
                "difficulty_desc" => enemiesIQ.OrderByDescending(d => d.CR).ThenByDescending(d => d.XP),
                _ => enemiesIQ.OrderByDescending(d => d.LastUpdated),
            };
            int pageSize = 7;
            Enemies = await PaginatedList<Enemy>.CreateAsync(
                enemiesIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
