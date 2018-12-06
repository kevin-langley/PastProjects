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
    [Authorize(Roles = "Manager")]
    public class ReordersController : Controller
    {
        private readonly AppDbContext _context;

        public ReordersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reorders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reorders.ToListAsync());
        }

        // GET: Reorders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reorder = await _context.Reorders
                .FirstOrDefaultAsync(m => m.ReorderID == id);
            if (reorder == null)
            {
                return NotFound();
            }

            return View(reorder);
        }

        // GET: Reorders/ManualReorder
        public IActionResult ManualReorder()
        {
            return View();
        }

        // POST: Reorders/ManualReorder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManualReorder([Bind("ReorderID")] Reorder reorder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reorder);
        }

        //TODO: Create Reorders/AutoReorder





        //POST
        //Method to process Add To Order results
        //Has form answers as paramater
        [HttpPost]
        public async Task<IActionResult> AddToReorder(BookReorder bo, int? bookId, int intReorderQuantity)
        {
            if (bookId == null)
            {
                return NotFound();
            }

            if (intReorderQuantity < 1)
            {
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            //Finding book matching book id passed from book details page
            Book book = _context.Books.Find(bookId);
            //Stores book in book order detail
            bo.Book = book;
            bo.ReorderQuantity = intReorderQuantity;

            //Finds if user already has an order pending
            //Assigning user to user id
            //get user info
            String id = User.Identity.Name;
            User user = _context.Users.Include(o => o.Reorders).ToList().FirstOrDefault(u => u.UserName == id); //NOTE: Relational data

            //If user has an order already
            if (user.Reorders.Count != 0)
            {
                if (user.Reorders.Exists(o => o.IsPending == true))
                {
                    //TODO

                    //Finds current pending order for this user and stores it in order
                    Reorder order = _context.Reorders.Include(us => us.User).Include(o => o.BookReorders).ThenInclude(o => o.Book).FirstOrDefault(u => u.User.UserName == user.UserName && u.IsPending == true);


                    BookReorder bookOrderToUpdate = new BookReorder();


                    //This is the path for updating a book already in the cart
                    try
                    {
                        //Iterating through list of book orders to add book order quantity instead of
                        //just adding a new book instance
                        foreach (BookReorder bookOrder in order.BookReorders)
                        {
                            if (bookOrder.Book == bo.Book)
                            {

                                bookOrder.ReorderQuantity = bookOrder.ReorderQuantity + bo.ReorderQuantity;
                                bookOrder.Price = bookOrder.Book.BookPrice;


                                bookOrderToUpdate = bookOrder;
                                bookOrderToUpdate.Reorder = order;


                                _context.Update(bookOrderToUpdate);
                                await _context.SaveChangesAsync();

                                ViewBag.AddedOrder = "Your order has been added!";
                                ViewBag.CartMessage = "View your cart below";

                                return RedirectToAction("Details", new { id = bookOrderToUpdate.Reorder.ReorderID });

                            }
                        }

                    }
                    catch
                    {
                        throw;
                    }

                    //This is the path for adding a new book to the cart
                    //Stores matched order in order detail order property
                    bo.Reorder = order;
                    bo.Price = bo.Book.BookPrice;
                    bo.ReorderQuantity = intReorderQuantity;          

                    _context.Add(bo);
                    await _context.SaveChangesAsync();

                    ViewBag.AddedOrder = "Your order has been added!";
                    ViewBag.CartMessage = "View your cart below";

                    return RedirectToAction("Details", new { id = bo.Reorder.ReorderID });

                }


                //This is the path if user does not have a pending order
                Reorder neworder = new Reorder();

                neworder.User = user;

                //Stores newly created order into order detail
                bo.Reorder = neworder;

                bo.Reorder.IsPending = true;

                //Stores most recently updated book price into order detail price
                //TODO: Check if we need to do this lol
                bo.Price = bo.Book.BookPrice;

                //Stores order quantity and necessary order details
                bo.ReorderQuantity = intReorderQuantity;

                if (ModelState.IsValid)
                {
                    _context.Add(neworder);
                    _context.BookReorders.Add(bo);
                    await _context.SaveChangesAsync();

                    ViewBag.AddedOrder = "Your order has been added!";
                    ViewBag.CartMessage = "View your cart below";

                    return RedirectToAction("Details", new { id = bo.Reorder.ReorderID });
                }

                return RedirectToAction("Details", "Books", new { id = bookId });
            }


            //This is the path if this is the user's first order
            Reorder firstorder = new Reorder();

            //Assigns user to order
            firstorder.User = user;

            //Stores order to order detail order navigational property
            bo.Reorder = firstorder;

            //Sets order to is pending so cart can persist
            bo.Reorder.IsPending = true;

            //Sets order quantity
            bo.ReorderQuantity = intReorderQuantity;

            //Stores most recently updated book price into order detail price & other stuff
            bo.Price = bo.Book.BookPrice;

            _context.Add(bo);
            await _context.SaveChangesAsync();

            ViewBag.AddedOrder = "Your order has been added!";
            ViewBag.CartMessage = "View your cart below";

            return RedirectToAction("Details", new { id = bo.Reorder.ReorderID });

        }

        //GET method to get order id for order
        public IActionResult RemoveFromReorder(int? id)
        {

            //Storing order into new order instance including relational data
            Reorder order = _context.Reorders.Include(r => r.BookReorders).ThenInclude(r => r.Book).FirstOrDefault(r => r.ReorderID == id);

            if (order == null || order.BookReorders.Count == 0)//order is not found
            {
                return RedirectToAction("Details", new { id = id });
            }

            return View(order.BookReorders); //Passes list of orderdetails for matching order id


        }

        //GET: Check out method
        public async Task<IActionResult> SendOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Reorders.Include(or => or.User).ThenInclude(or => or.CreditCards).FirstOrDefaultAsync(m => m.ReorderID == id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        //POST: Check out method processed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder(int id, Reorder order)
        {
            if (id != order.ReorderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Int32 intTotalBookNum = 0;

                try
                {
                    order = _context.Reorders
                                .Include(or => or.BookReorders)
                                .Include(u => u.User).FirstOrDefault(o => o.ReorderID == id);

                    
                    
                    

                    foreach (BookReorder bo in order.BookReorders)
                    {
                        intTotalBookNum = intTotalBookNum + bo.ReorderQuantity;
                    }


                    

                    _context.Update(order);
                    await _context.SaveChangesAsync();

                    return View("ConfirmSendOrder", order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReorderExists(order.ReorderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                
            }
            return View(order);
        }

        private bool ReorderExists(int id)
        {
            return _context.Reorders.Any(e => e.ReorderID == id);
        }

        //This is the method for placing an order
        //If correctly functioning, this order should update all book stocks by removing quantity
        //This method should also update the shopping cart
        //TODO: Make sure this works correctly
        public async Task<IActionResult> PlaceOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Finding order matching order passed in and storing it in order object
                Reorder order = _context.Reorders
                                    .Include(or => or.BookReorders).ThenInclude(or => or.Book)
                                    .Include(u => u.User)
                                    .FirstOrDefault(o => o.ReorderID == id);


                Book bookUpdate = new Book();

                //Iterates thr
                foreach (BookReorder bo in order.BookReorders)
                {
                    //Matching book object with book for that specific book order detail
                    bookUpdate = _context.Books.Find(bo.Book.BookID);

                    //Subtracts book order quantity from current copies on hand value for this book 
                    bookUpdate.CopiesOnHand = bookUpdate.CopiesOnHand + bo.ReorderQuantity;

                    //Update changes in database
                    _context.Update(bookUpdate);
                    await _context.SaveChangesAsync();
                }

                //Closes order so cannot show up in shopping cart anymore
                order.IsPending = false;

                _context.Update(order);
                await _context.SaveChangesAsync();

                return View("CompletedOrder");

            }
            //Sad path :(
            return RedirectToAction("CartDetails");


        }

    }
}
