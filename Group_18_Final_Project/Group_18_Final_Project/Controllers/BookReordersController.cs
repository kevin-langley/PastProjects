using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;
using Microsoft.AspNetCore.Authorization;


//Controller concerns books in the cart
namespace Group_18_Final_Project.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BookReordersController : Controller
    {
        private readonly AppDbContext _context;

        public BookReordersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BookReOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookReorders.ToListAsync());
        }

        // GET: BookReOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookOrder = await _context.BookReorders
                .FirstOrDefaultAsync(m => m.BookReorderID == id);
            if (bookOrder == null)
            {
                return NotFound();
            }

            return View(bookOrder);
        }

        // GET: BookOrders/ManualReorder
        public IActionResult ManualReorder()
        {
            return View();
        }

        // POST: BookreOrders/ManualReorder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManualReorder([Bind("BookReorderID,Price,ReorderQuantity")] BookReorder bookOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookOrder);
        }        

        
    }
}