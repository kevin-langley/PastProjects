using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;


//Controller concerns books in the cart
namespace Group_18_Final_Project.Controllers
{
    public class BookOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public BookOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BookOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookOrders.ToListAsync());
        }

        // GET: BookOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders
                .FirstOrDefaultAsync(m => m.BookOrderID == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }

        // GET: BookOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookOrderID,Price,OrderQuantity")] BookOrder bookOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookOrder);
        }

        // GET: BookOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders.FindAsync(id);
            ViewBag.BookTitle = bookOrder.Book.Title;
            if (bookOrder == null)
            {
                return NotFound();
            }
            return View(bookOrder);
        }

        // POST: BookOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int OrderQuantity, BookOrder bookOrder)
        {
            if (id != bookOrder.BookOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Find the related order detail in the database
                    BookOrder dbbookOrder = _context.BookOrders
                                                        .Include(bo => bo.Book)
                                                        .Include(bo => bo.Order)
                                                        .FirstOrDefault(bo => bo.BookOrderID ==
                                                                        bookOrder.BookOrderID);

                    //Updating field
                    dbbookOrder.OrderQuantity = OrderQuantity;
                    dbbookOrder.Price = dbbookOrder.Book.BookPrice; //Most-updated book price
                    dbbookOrder.ExtendedPrice = dbbookOrder.Price * dbbookOrder.OrderQuantity;

                    //Making update in db
                    _context.BookOrders.Update(dbbookOrder);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Details", "Orders", new { id = dbbookOrder.Order.OrderID });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookOrderExists(bookOrder.BookOrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(bookOrder);
        }

        // GET: BookOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookOrders
                .FirstOrDefaultAsync(m => m.BookOrderID == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }

        // POST: BookOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookOrder = await _context.BookOrders.FindAsync(id);
            _context.BookOrders.Remove(bookOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookOrderExists(int id)
        {
            return _context.BookOrders.Any(e => e.BookOrderID == id);
        }
    }
}
