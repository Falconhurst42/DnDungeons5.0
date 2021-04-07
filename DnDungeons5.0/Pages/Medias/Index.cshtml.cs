using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DnDungeons.Data;
using DnDungeons.Models;

namespace DnDungeons.Pages.Medias
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

        public PaginatedList<Media> Medias { get;set; }

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

            IQueryable<Media> mediasIQ = from m in _context.Medias
                                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                mediasIQ = mediasIQ.Where(e => e.Name.Contains(searchString)
                                       || e.Description.Contains(searchString));
            }

            mediasIQ = sortOrder switch
            {
                "name" => mediasIQ.OrderBy(d => d.Name),
                "name_desc" => mediasIQ.OrderByDescending(d => d.Name),
                "date_u_old" => mediasIQ.OrderBy(d => d.LastUpdated),
                "date_c" => mediasIQ.OrderByDescending(d => d.Created),
                "date_c_old" => mediasIQ.OrderBy(d => d.Created),
                _ => mediasIQ.OrderByDescending(d => d.LastUpdated),
            };
            int pageSize = 7;
            Medias = await PaginatedList<Media>.CreateAsync(
                mediasIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
