using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BookStore.Pages.Admin.Books
{
    public class CreateModel : CategoryAndAuthorNamePageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateModel(BookStore.Data.BookStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public  IActionResult OnGet()
        {
            PopulateAuthorsDropDownList(_context);
            PopulateCategoriesDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Book Book { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Book book = new Book();
            if(Book.FormFile != null)
            {
                IFormFile formFile = Book.FormFile;
                if (formFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    book.Cover = uniqueFileName;

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            else
            {
                book.Cover = "book-covers.jpg";
            }
            

            book.CreateDate = DateTime.Now;
            book.ModifedDate = DateTime.Now;
            if (await TryUpdateModelAsync<Book>(
                book,
                "book",
                b => b.Title,b=>b.Summary,b=>b.Price,b=>b.AuthorID,b=>b.CategoryID))
            {
                _context.Book.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAuthorsDropDownList(_context);
            PopulateCategoriesDropDownList(_context);
            return Page();
        }
    }
}
