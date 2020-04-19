using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace BookStore.Pages.Admin.Books
{
    public class EditModel : CategoryAndAuthorNamePageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(BookStore.Data.BookStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Book Book { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            PopulateAuthorsDropDownList(_context, Book.AuthorID);
            PopulateCategoriesDropDownList(_context, Book.AuthorID);

            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (Book.FormFile != null)
            {
                IFormFile formFile = Book.FormFile;
                if (formFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    FileInfo fileInfo = new FileInfo(Path.Combine(uploadsFolder,book.Cover));
                    if (book.Cover != "book-covers.jpg" && fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                    book.Cover = uniqueFileName;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            

            if (book == null)
            {
                return NotFound();
            }
            book.ModifedDate = DateTime.Now;
            if (await TryUpdateModelAsync<Book>(
                 book,
                 "book",   // Prefix for form value.
                    b => b.Title, b => b.Summary, b => b.Price, b => b.AuthorID, b => b.CategoryID))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAuthorsDropDownList(_context, Book.AuthorID);
            PopulateCategoriesDropDownList(_context, Book.AuthorID);
            return Page();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
