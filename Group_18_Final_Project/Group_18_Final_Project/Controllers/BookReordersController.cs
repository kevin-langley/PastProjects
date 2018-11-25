using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;

namespace Group_18_Final_Project.Controllers
{
    public class BookReordersController : Controller
    {
        private readonly AppDbContext _context;

        public BookReordersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BookReorders
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookReorders.ToListAsync());
        }

        // GET: BookReorders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReorder = await _context.BookReorders
                .FirstOrDefaultAsync(m => m.BookReorderID == id);
            if (bookReorder == null)
            {
                return NotFound();
            }

            return View(bookReorder);
        }

        // GET: BookReorders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookReorders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookReorderID,ReorderQuantity")] BookReorder bookReorder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookReorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookReorder);
        }

        // GET: BookReorders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReorder = await _context.BookReorders.FindAsync(id);
            if (bookReorder == null)
            {
                return NotFound();
            }
            return View(bookReorder);
        }

        // POST: BookReorders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookReorderID,ReorderQuantity")] BookReorder bookReorder)
        {
            if (id != bookReorder.BookReorderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookReorder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookReorderExists(bookReorder.BookReorderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookReorder);
        }

        // GET: BookReorders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookReorder = await _context.BookReorders
                .FirstOrDefaultAsync(m => m.BookReorderID == id);
            if (bookReorder == null)
            {
                return NotFound();
            }

            return View(bookReorder);
        }

        // POST: BookReorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookReorder = await _context.BookReorders.FindAsync(id);
            _context.BookReorders.Remove(bookReorder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookReorderExists(int id)
        {
            return _context.BookReorders.Any(e => e.BookReorderID == id);
        }
    }
}
