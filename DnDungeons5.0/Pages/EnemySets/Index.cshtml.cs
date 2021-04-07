using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.EnemySets
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

        public PaginatedList<EnemySet> EnemySets { get;set; }

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

            IQueryable<EnemySet> enemySetsIQ = from es in _context.EnemySets
                                               select es;

            if (!String.IsNullOrEmpty(searchString))
            {
                enemySetsIQ = enemySetsIQ.Where(es => es.Name.Contains(searchString)
                                       || es.Description.Contains(searchString));
            }

            enemySetsIQ = sortOrder switch
            {
                "name" => enemySetsIQ.OrderBy(es => es.Name),
                "name_desc" => enemySetsIQ.OrderByDescending(es => es.Name),
                "date_u_old" => enemySetsIQ.OrderBy(d => d.LastUpdated),
                "date_c" => enemySetsIQ.OrderByDescending(d => d.Created),
                "date_c_old" => enemySetsIQ.OrderBy(d => d.Created),
                _ => enemySetsIQ.OrderByDescending(es => es.LastUpdated),
            };
            int pageSize = 7;
            EnemySets = await PaginatedList<EnemySet>.CreateAsync(
                enemySetsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
