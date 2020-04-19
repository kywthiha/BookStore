using BookStore.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Pages.Admin.Books
{
    public class CategoryAndAuthorNamePageModel : PageModel
    {
        public SelectList Categories { get; set; }
        public SelectList Authors { get; set; }

        public void PopulateCategoriesDropDownList(BookStoreContext _context,
            object selectedCategory = null)
        {
            var categoriesQuery = from c in _context.Category
                                   orderby c.Title
                                   select c;

            Categories = new SelectList(categoriesQuery.AsNoTracking(),
                        "ID", "Title", selectedCategory);


        }
        public void PopulateAuthorsDropDownList(BookStoreContext _context,
            object selectedAuthor = null)
        {
            var authorsQuery = from a in _context.Author
                                  orderby a.Name
                                  select a;

            Authors = new SelectList(authorsQuery.AsNoTracking(),
                        "ID", "Name", selectedAuthor);
        }
    }
}
