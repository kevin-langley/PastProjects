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

namespace Group_18_Final_Project.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        //Create new list to find only approved reviews
        public async Task<IActionResult> Index(int? id)
        {
            if (User.IsInRole("Customer"))
            {
                if (id == null)
                {
                    return NotFound();
                }
                //if book id is specified
                List<Review> bookreviews = await _context.Reviews
                                            .Include(u => u.Author)
                                            .Include(u => u.Book)
                                            .Where(u => u.Book.BookID == id).ToListAsync();

                return View(bookreviews);
            }
            if((User.IsInRole("Manager") || User.IsInRole("Employee")) == true)
            {
                if (id == null)
                {
                    return View(await _context.Reviews.ToListAsync());
                }
                //if book id is specified
                List<Review> bookReviews = await _context.Reviews
                    .Include(u => u.Author)
                    .Include(u => u.Book)
                    .Where(u => u.Book.BookID == id).ToListAsync();
                return View(bookReviews);

            }
            //View page for all approved reviews
            if (id == null)
            {
                return View(await _context.Reviews.Where(r => r.Approval == true).ToListAsync());
            }

            //if book id is specified
            List<Review> reviews = await _context.Reviews
                                        .Include(u => u.Author)
                                        .Include(u => u.Book)
                                        .Where(u => u.Book.BookID == id).ToListAsync();

            return View(reviews);

        }
        
        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public IActionResult Create(int? id)
        {
            string userid = User.Identity.Name;
            User user = _context.Users
                                .Include(u => u.Orders).ThenInclude(u => u.BookOrders).FirstOrDefault(u => u.UserName == userid);

            List<BookOrder> bookOrders = _context.BookOrders
                                                    .Include(bo => bo.Book)
                                                    .Where(bo => bo.Order.User == user).ToList();

            if (bookOrders.Any(bo => bo.Book.BookID == id))
            {
                return View();
            }

            return RedirectToAction("Details", "Books", new { id });
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,ReviewText,Rating")] Review review, int id)
        {
            if (ModelState.IsValid)
            {
                string userid = User.Identity.Name;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userid);

                Book book = await _context.Books.FirstOrDefaultAsync(b => b.BookID == id);

                review.Book = book;
                review.IsPending = true;
                review.Approval = false;
                review.Author = user;

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,ReviewText,Approval,Rating")] Review review)
        {
            if (id != review.ReviewID)
            {
                return NotFound();
            }
            if ((User.IsInRole("Employee") == false || User.IsInRole("Manager") == false )&& review.Author.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to edit this review!" });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }

        [Authorize(Roles = "Manager, Employee")]
        public IActionResult ApproveReviews()
        {
            List<Review> reviewsToApprove = _context.Reviews
                                                        .Include(u => u.Author)
                                                        .Include(r => r.Book)
                                                        .Where(r => r.IsPending == true).ToList();

            return View(reviewsToApprove);
        }

        public async Task<IActionResult> CheckedReviews(Boolean checkedBox)
        {
            List<Review> checkedReviews = _context.Reviews
                                                        .Include(u => u.Author)
                                                        .Include(r => r.Book)
                                                        .Where(r => r.IsPending == true).ToList();

            string id = User.Identity.Name;
            User approver = await _context.Users.FirstOrDefaultAsync(u => u.UserName == id);

            foreach (Review review in checkedReviews)
            {
                if (checkedBox == true)
                {
                    review.IsPending = false;
                    review.Approval = true;
                    review.Author = approver;

                    _context.Reviews.Update(review);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }

                //if review is rejected
                review.IsPending = false;
                review.Author = approver;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Index");

        }
    }
}
