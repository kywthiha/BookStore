using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public IndexModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchEmail { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPhone { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchStatus { get; set; }


        public async Task<ActionResult> OnGetAsync(int? mid,int? uid)
        {
            var orders = from o in _context.Order
                        select o;
            if (!string.IsNullOrEmpty(SearchName))
            {
                orders = orders.Where(c => c.Name.Contains(SearchName));
            }
            if (!string.IsNullOrEmpty(SearchEmail))
            {
                orders = orders.Where(c => c.Email.Contains(SearchName));
            }
            if (!string.IsNullOrEmpty(SearchPhone))
            {
                orders = orders.Where(c => c.Phone.Contains(SearchPhone));
            }
            if (!string.IsNullOrEmpty(SearchStatus))
            {
                bool status = Convert.ToBoolean(SearchStatus);
                orders = orders.Where(c => c.Status==status);
            }
            if (!string.IsNullOrEmpty(SearchDate))
            {
                DateTime? mydate = Convert.ToDateTime(SearchDate);
                DateTime? myDateTomorrow = mydate.Value.AddDays(1);
                orders = orders.Where(c => c.CreateDate >= mydate && c.CreateDate < myDateTomorrow);
            }
            Order = await orders
                .Include(o=>o.OrderItems)
                .ThenInclude(or=>or.Book)
                .OrderBy(o=>o.Status)
                .ToListAsync();
            if(mid != null)
            {
                var order = await _context.Order.FindAsync(mid);

                if (order == null)
                {
                    return NotFound();
                }
                order.ModifedDate = DateTime.Now;
                order.Status = true;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            if (uid != null)
            {
                var order = await _context.Order.FindAsync(uid);

                if (order == null)
                {
                    return NotFound();
                }
                order.ModifedDate = DateTime.Now;
                order.Status = false;
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }


            return Page();
        }
    }
}
