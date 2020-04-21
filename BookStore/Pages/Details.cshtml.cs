using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public DetailsModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }

        public Book Book { get; set; }
        public IDictionary<string, int> BookCats { get; set; }
        public string BookCatsInfo { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id,int? addCatId)
        {
            BookCatsInfo = SessionExtensions.GetCounts(HttpContext.Session).ToString();
            if (id == null)
            {
                return NotFound();
            }
            if (addCatId != null)
            {
                string key = addCatId.Value.ToString();
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
                return Redirect("/Details?id="+id);


            }
            Book = await _context
                .Book
                .Include(b=>b.Author)
                .Include(b=>b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
