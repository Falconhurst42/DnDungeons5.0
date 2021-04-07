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
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public Layout Layout { get; set; }

        public ICollection<LayoutMedia> LayoutMedias { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Layout = await _context.Layouts.FirstOrDefaultAsync(m => m.ID == id);

            if (Layout == null)
            {
                return NotFound();
            }

            // load related Media links and Media
            // not sure why this didn't work for Dungeons, but this is a more intended manner of loading related data
            LayoutMedias = await _context.LayoutMedias
                .Where(em => em.LayoutID == id)
                .Include(em => em.Media)
                .ToListAsync();

            return Page();
        }
    }
}
