using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using DnDungeons.Data;
using DnDungeons.Models;
using DnDungeons.Utilities;

// file uploading strategy courtesy of the documentation: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0, https://github.com/dotnet/AspNetCore.Docs/blob/master/aspnetcore/mvc/models/file-uploads/samples/3.x/SampleApp/Pages/BufferedSingleFileUploadDb.cshtml.cs

namespace DnDungeons.Pages.Medias
{
    public class CreateModel : PageModel
    {
        private readonly DnDungeons.Data.DnDungeonsContext _context;
        private readonly long _fileSizeLimit = 512000;
        private readonly string[] _permittedExtensions = { ".png", ".jpg", ".jpeg" };

        public CreateModel(DnDungeons.Data.DnDungeonsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public Media Media { get; set; }

        public string ErrorMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostUploadAsync()
        {
            // Perform an initial check to catch FileUpload class
            // attribute violations.
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the form.";

                return Page();
            }

            var formFileContent =
                await FileHelpers.ProcessFormFile<IFormFile>(
                    FileUpload, ModelState, _permittedExtensions,
                    _fileSizeLimit);

            // Perform a second check to catch ProcessFormFile method
            // violations. If any validation check fails, return to the
            // page.
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please correct the form.";

                return Page();
            }

            var emptyMedia = new Media();

            // emptyEnemy.Created = DateTime.UtcNow;

            if (await TryUpdateModelAsync<Media>(
                emptyMedia,
                "media",   // Prefix for form value.
                d => d.Name, d => d.Description))
            {
                emptyMedia.File = formFileContent;

                _context.Medias.Add(emptyMedia);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
