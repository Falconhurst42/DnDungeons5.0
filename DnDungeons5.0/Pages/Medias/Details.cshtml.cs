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
    public class DetailsModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;

        public DetailsModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public Media Media { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Media = await _context.Medias.FindAsync(id);

            if (Media == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
