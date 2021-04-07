using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Layouts
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

        public PaginatedList<Layout> Layouts { get; set; }

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

            IQueryable<Layout> layoutsIQ = from l in _context.Layouts
                                             select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                layoutsIQ = layoutsIQ.Where(d => d.Name.Contains(searchString)
                                       || d.Description.Contains(searchString));
            }

            layoutsIQ = sortOrder switch
            {
                "name" => layoutsIQ.OrderBy(d => d.Name),
                "name_desc" => layoutsIQ.OrderByDescending(d => d.Name),
                "date_u_old" => layoutsIQ.OrderBy(d => d.LastUpdated),
                "date_c" => layoutsIQ.OrderByDescending(d => d.Created),
                "date_c_old" => layoutsIQ.OrderBy(d => d.Created),
                _ => layoutsIQ.OrderByDescending(d => d.LastUpdated),
            };
            int pageSize = 7;
            Layouts = await PaginatedList<Layout>.CreateAsync(
                layoutsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
