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
    public class DetailsModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public DetailsModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id,int? mid,int? uid)
        {
            if (id == null && mid == null && uid == null)
            {
                return NotFound();
            }
            Order = await _context.Order
                .Include(o=>o.OrderItems)
                .ThenInclude(or=>or.Book)
                .FirstOrDefaultAsync(m => m.ID == (id!=null?id:mid!=null?mid:uid));

            if (Order == null)
            {
                return NotFound();
            }
            if (mid != null)
            {
                var order = await _context.Order.FindAsync(mid);

                if (order == null)
                {
                    return NotFound();
                }
                order.ModifedDate = DateTime.Now;
                order.Status = true;
                await _context.SaveChangesAsync();
                return Redirect("./Details?id=" + mid);
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
                return Redirect("./Details?id=" + uid);
            }
            return Page();
        }
    }
}
