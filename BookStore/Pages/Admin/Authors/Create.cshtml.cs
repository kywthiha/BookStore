﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Pages.Admin.Authors
{
    public class CreateModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public CreateModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Author Author { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Author author = new Author();
            author.CreateDate = DateTime.Now;
            author.ModifedDate = DateTime.Now;
            if (await TryUpdateModelAsync<Author>(
                author,
                "author",
                a => a.Name))
            {
                _context.Author.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}