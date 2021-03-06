﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace Group_18_Final_Project.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Book book  = await _context.Books.Include(b => b.Genre)
                .Include(bo => bo.BookOrders)
                .ThenInclude(o => o.Order)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(m => m.BookID == id);
            List<BookOrder> bookOrders = book.BookOrders.Where(b => b.Order.IsPending == true).ToList();
            bookOrders = bookOrders.Where(b => b.Order.User.UserName == User.Identity.Name).ToList();
            
            if (book == null)
            {
                return NotFound();
            }

            //NOTE: Check this

            ////calculate average rating
            //try
            //{
            //    int totalRating = book.Reviews.Where(item => item.Approval == true).Sum(item => item.Rating);
            //    int numberReviews = book.Reviews.Where(item => item.Approval == true).Count();
            //    //book.AverageRating = (totalRating / numberReviews).ToString();
            //}
            //catch
            //{
            //    book.AverageRating = "This book does not have any reviews yet!";
            //}

            //Display whether book is already in customer's cart or not
            int intOrderQuantity = bookOrders.Sum(o => o.OrderQuantity);
            ViewBag.InCart = intOrderQuantity > 0 ? "This book is already in your cart" : "This book is not yet in your cart";


            string userid = User.Identity.Name;
            User user = await _context.Users
                                        .Include(u => u.Orders)
                                        .FirstOrDefaultAsync(u => u.UserName == userid);

            List<Order> orders = _context.Orders.Include(bo => bo.BookOrders).Include(bo => bo.User).Where(u => u.User == user).ToList();


            foreach (Order order in orders)
            {
                if (!order.BookOrders.All(bo => bo.Book == book))
                {
                    ViewBag.NoOrder = "Sorry! You can't review a book you have not purchased.";

                }
            }

            //List of reviews that this user has made
            List<Review> userReviews = _context.Reviews.Include(u => u.Book).Where(u => u.Author == user).ToList();

            foreach (Review review in userReviews)
            {
                if (review.Book.BookID == id)
                {
                    ViewBag.AlreadyReviewed = "Sorry! You've already written a review for this book.";
                }
            }


            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Author,UniqueID,TimesPurchased,AverageRating,CopiesOnHand,BookPrice,WholesalePrice,ActiveBook,PublicationDate,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles="Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Author,UniqueID,TimesPurchased,AverageRating,CopiesOnHand,BookPrice,WholesalePrice,ActiveBook,PublicationDate,Description")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }

    }
}
