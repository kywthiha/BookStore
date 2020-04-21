﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Newtonsoft.Json;

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
            Book = await _context
                .Book
                .Include(b=>b.Author)
                .Include(b=>b.Category)
                .AsNoTracking()
                .ToListAsync();
            return Page();
        }
    }
}
