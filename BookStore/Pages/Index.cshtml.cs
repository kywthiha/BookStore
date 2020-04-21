using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public IndexModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }

        public IList<Book> Book { get;set; }
        public string BookCatsInfo { get; set; }
        public IDictionary<string, int> BookCats { get ; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Authors { get; set; }
        public SelectList Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string BookAuthor { get; set; }
        [BindProperty(SupportsGet = true)]
        public string BookCategory { get; set; }

        public async Task<ActionResult> OnGetAsync(int? id,int? clearData)
        {

            if (clearData != null && clearData.Value == 1)
            {
                HttpContext.Session.Remove("bookCats");
            }
            if (id != null)
            {
                string key = id.Value.ToString();
                BookCats = SessionExtensions.Get<IDictionary<string, int>>(HttpContext.Session, "bookCats");
                if (BookCats == null)
                {
                    BookCats = new Dictionary<string, int>();
                }
                if (BookCats.ContainsKey(key))
                {
                    BookCats[key]++;
                }
                else
                {
                    BookCats.Add(key, 1);
                }

                SessionExtensions.Set<IDictionary<string, int>>(HttpContext.Session, "bookCats", BookCats);
                return Redirect("/");
                
                
            }
            BookCatsInfo = SessionExtensions.GetCounts(HttpContext.Session).ToString();

            var books = from b in _context.Book
                        join c in _context.Category on b.CategoryID equals c.ID
                        join a in _context.Author on b.AuthorID equals a.ID
                        select b;
            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(b=>b.Title.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(BookAuthor))
            {
                Int64 tempid = Convert.ToInt64(BookAuthor);
                books = books.Where(b=>b.AuthorID == tempid);
            }
            if (!string.IsNullOrEmpty(BookCategory))
            {
                Int64 tempid = Convert.ToInt64(BookCategory);
                books = books.Where(b => b.CategoryID == tempid);
            }
            var authorsQuery = from a in _context.Author
                                  orderby a.Name
                                  select a;

            Authors = new SelectList(authorsQuery.AsNoTracking(),
                        "ID", "Name");
            var categoriesQuery = from c in _context.Category
                                  orderby c.Title
                                  select c;

            Categories = new SelectList(categoriesQuery.AsNoTracking(),
                        "ID", "Title");
            Book = await books
                .Include(b=>b.Author)
                .Include(b=>b.Category)
                .AsNoTracking()
                .ToListAsync();
            return Page();
        }
    }
}
