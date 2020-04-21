using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Pages
{
    public class CreateModel : PageModel
    {
        private readonly BookStore.Data.BookStoreContext _context;

        public CreateModel(BookStore.Data.BookStoreContext context)
        {
            _context = context;
        }
        public string BookCatsInfo { get; set; }

        public IDictionary<string, int> BookCats { get; set; }

        public IList<Book> Books { get; set; }

        public IList<Book> OrderBookList()
        {
            BookCats = SessionExtensions.Get<IDictionary<string, int>>(HttpContext.Session, "bookCats");

            if (BookCats != null && BookCats.Count > 0)
            {
                var idlist = new Int64[BookCats.Count];
                int i = 0;
                foreach (var kv in BookCats)
                {
                    idlist[i++] = Convert.ToInt64(kv.Key);
                }
                var ook = from b in _context.Book
                          where idlist.Contains(b.ID)
                          select b;
                foreach (var kv in ook)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.Qty = BookCats[kv.ID.ToString()];
                    orderItem.Total = kv.Price * orderItem.Qty;
                    kv.OrderItem = orderItem;
                }
                return ook.ToList();
            }
            return null;
        }

        public IActionResult OnGet()
        {
            BookCatsInfo = SessionExtensions.GetCounts(HttpContext.Session).ToString();
            Books = OrderBookList();
            if (Books != null) { 
                return Page();
            }
            return Redirect("/");
        }

        [BindProperty]
        public Order Order { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Order order = new Order();
            DateTime helloNow = DateTime.Now;
            order.CreateDate = helloNow;
            order.ModifedDate = helloNow;
            Books = OrderBookList();
            if (Books == null)
            {
                return Page();
            }
            if (await TryUpdateModelAsync<Order>(
                order,
                "order",
                o=>o.Name,o=>o.Email,o=>o.Phone,o=>o.Address))
            {
                _context.Order.Add(order);
                await _context.SaveChangesAsync();
                int orderId = order.ID;
                foreach ( var book in Books)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.BookID = book.ID;
                    orderItem.OrderID = orderId;
                    orderItem.Qty = book.OrderItem.Qty;
                    _context.OrderItem.Add(orderItem);
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("bookCats");
                return RedirectToPage("./Index");
            }
          
            return Page();
        }
    }
}
