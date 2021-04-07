using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Loots
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
        public string IsMoneySort { get; set; }
        public string ValueSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Loot> Loots { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            DateUpdatedSort = String.IsNullOrEmpty(sortOrder) ? "date_u_old" : "";
            NameSort = (sortOrder == "name") ? "name_desc" : "name";
            DateCreatedSort = (sortOrder == "date_c") ? "date_c_old" : "date_c";
            IsMoneySort = (sortOrder == "is_money") ? "is_money_desc" : "is_money";
            ValueSort = (sortOrder == "value") ? "value_desc" : "value";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Loot> lootsIQ = from l in _context.Loots
                                             select l;

            if (!String.IsNullOrEmpty(searchString))
            {
                lootsIQ = lootsIQ.Where(l => l.Name.Contains(searchString)
                                       || l.Description.Contains(searchString)) ;
            }

            lootsIQ = sortOrder switch
            {
                "name" => lootsIQ.OrderBy(d => d.Name),
                "name_desc" => lootsIQ.OrderByDescending(d => d.Name),
                "date_u_old" => lootsIQ.OrderBy(d => d.LastUpdated),
                "date_c" => lootsIQ.OrderByDescending(d => d.Created),
                "date_c_old" => lootsIQ.OrderBy(d => d.Created),
                "is_money" => lootsIQ.Where(d => d.IsMoney),
                "is_money_desc" => lootsIQ.Where(d => !d.IsMoney),
                "value" => lootsIQ.OrderBy(d => d.MonetaryValue),
                "value_desc" => lootsIQ.OrderByDescending(d => d.MonetaryValue),
                _ => lootsIQ.OrderByDescending(d => d.LastUpdated),
            };
            int pageSize = 7;
            Loots = await PaginatedList<Loot>.CreateAsync(
                lootsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
