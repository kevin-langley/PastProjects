using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            //get user info
            List<Reorder> UserOrders = new List<Reorder>();
            String id = User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.UserName == id); 


            //TODO: Remove admin@example.com from customer role!!
                UserOrders = await _context.Reorders.Include(o => o.BookReorders).Where(o => o.IsPending == false).ToListAsync();
            
            return View(UserOrders);

        }
        //GET: Orders/Details
        //For the navbar and anytime you can't route an id
        public IActionResult CartDetails()
        {
            string id = User.Identity.Name;
            User user = _context.Users.Include(us => us.Reorders).FirstOrDefault(u => u.UserName == id);

            if (user.Reorders == null || user.Reorders.Exists(o => o.IsPending == false))
            {
                ViewBag.EmptyMessage = "Looks like your order is empty! Search for books to add.";
                return View("EmptyCart");
            }
            else
            {
                if (user.Reorders.Exists(o => o.IsPending == true))
                {
                    //Finds order in db matching user
                    Reorder order = _context.Reorders.Include(us => us.User).Include(o => o.BookReorders).ThenInclude(o => o.Book).FirstOrDefault(u => u.User.UserName == user.UserName && u.IsPending == true);

                    return RedirectToAction("Details", new { id = order.ReorderID });
                }

            }
            ViewBag.EmptyMessage = "Looks like your order is empty! Search for books to add.";
            return View("EmptyCart");

        }

        // GET: Orders/Details/5
        //This is the shopping cart
        public async Task<IActionResult> Details(int? id)
        {
            if (ModelState.IsValid) //Happy path!
            {
                try
                {
                    //For shipping
                    int intTotalBookNum = 0;

                    //queries db to find user's order
                    //includes relational data for book order
                    var order = await _context.Reorders.Include(bo => bo.BookReorders).ThenInclude(bo => bo.Book)
                        .FirstOrDefaultAsync(m => m.ReorderID == id);

                    List<BookReorder> BooksToRemove = new List<BookReorder>();

                    //NOTE: Thinking that the code to remove items
                    //from the cart according to stock/active books should go here
                    //Subject to change
                    foreach (BookReorder bookOrder in order.BookReorders)
                    {
                        //NOTE: need to check if this logic makes sense
                        //If a book is both out of stock and inactive
                        //would removing it and then requesting to remove it again
                        //make an error? Adding extra code so the remove action
                        //doesn't happen twice

                        //Bool var check to remove order detail
                        bool boolRemoveBook = false;

                        if (bookOrder.Book.ActiveBook == false)
                        {
                            ViewBag.DiscontinuedBook = "Sorry! " + bookOrder.Book.Title + " has been discontinued and we cannot order it.";
                            boolRemoveBook = true;
                        }

                        if (boolRemoveBook == true)
                        {
                            BooksToRemove.Add(bookOrder);
                        }
                    }

                    //Remove books found in list above
                    foreach (BookReorder bo in BooksToRemove)
                    {
                        _context.BookReorders.Remove(bo);
                        _context.SaveChanges();
                    }

                    foreach (BookReorder bo in order.BookReorders)
                    {
                        intTotalBookNum = intTotalBookNum + bo.ReorderQuantity;
                    }

                    return View(order);

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                //Sad Path :( model state is not valid
            }

            return View("Index", "Home");
        }

        //This action displays details of completed orders
        //Has order id as parameter
        public async Task<IActionResult> CompletedReorderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Finding current logged in user to match user to order id
            string userid = User.Identity.Name;
            User user = _context.Users.FirstOrDefault(us => us.UserName == userid);

            //Finds order (including relational data) of the order matched in the order id
            Reorder order = _context.Reorders
                                .Include(or => or.BookReorders).ThenInclude(bo => bo.Book)
                                .Where(or => or.User == user).FirstOrDefault(or => or.ReorderID == id);


            if (ModelState.IsValid)
            {


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



                //pass matched order to view
                return View(order);
            }
            else { return RedirectToAction("Details", new { id = id }); }
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

        // GET: Reorders/AutoReorder
        public IActionResult AutoReorder()
        {
            List<Book> reordersToApprove = _context.Books
                                                        .Include(u => u.BookOrders)

                                                        .Where(r => r.ReorderPoint >= r.CopiesOnHand).ToList();

            return View(new ReorderUpdateModel { ToUpdate = reordersToApprove });
        }

        // POST: Reorders/AutoReorder
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoReorder(ReorderUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                List<Book> books = await _context.Books.Where(o => o.CopiesOnHand <= o.ReorderPoint).ToListAsync();

                BookReorder neworder = new BookReorder();

                //var books = from e in _context.Books
                //            from p in _context.Books
                //            where e.ReorderPoint >= p.CopiesOnHand
                //            select p;

                foreach(Book x in books)
                {
                    await AddToReorder(neworder, x.BookID, 5);
                }
                

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }




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

        //AutoReorderCreate
        public async Task<IActionResult> AutoReorderCreate(BookReorder bo, int? bookId, int intReorderQuantity)
        {
            Reorder neworder = new Reorder();


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


        

        private bool ReorderExists(int id)
        {
            return _context.Reorders.Any(e => e.ReorderID == id);
        }

        

    }
}
