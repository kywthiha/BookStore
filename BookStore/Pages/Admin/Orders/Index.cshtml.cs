using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;

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

        public async Task<ActionResult> OnGetAsync(int? mid,int? uid)
        {
            Order = await _context.Order
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
