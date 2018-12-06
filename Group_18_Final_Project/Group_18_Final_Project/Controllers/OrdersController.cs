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
    public enum CheckOutChoice { CurrentCard, NewCard };
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {

            //get user info
            List<Order> UserOrders = new List<Order>();
            String id = User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.UserName == id);

            if (User.IsInRole("Customer"))
            {
                UserOrders = await _context.Orders
                                            .Include(or => or.BookOrders)
                                            .Where(o => o.User.UserName == User.Identity.Name && o.IsPending == false).ToListAsync();

            }

            //TODO: Remove admin@example.com from customer role!!
            else //user is manager and can see all orders
            {
                UserOrders = await _context.Orders.Include(o => o.BookOrders).Where(o => o.IsPending == false).ToListAsync();
            }
            return View(UserOrders);

        }

        //GET: Orders/Details
        //For the navbar and anytime you can't route an id
        public IActionResult CartDetails()
        {
            string id = User.Identity.Name;
            User user = _context.Users.Include(us => us.Orders).FirstOrDefault(u => u.UserName == id);

            if (user.Orders == null || user.Orders.Exists(o => o.IsPending == false))
            {
                ViewBag.EmptyMessage = "Looks like your cart is empty! Search for books to add.";
                return View("EmptyCart");
            }
            else
            {
                if (user.Orders.Exists(o => o.IsPending == true))
                {
                    //Finds order in db matching user
                    Order order = _context.Orders.Include(us => us.User).Include(o => o.BookOrders).ThenInclude(o => o.Book).FirstOrDefault(u => u.User.UserName == user.UserName && u.IsPending == true);

                    return RedirectToAction("Details", new { id = order.OrderID });
                }

            }
            ViewBag.EmptyMessage = "Looks like your cart is empty! Search for books to add.";
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
                    var order = await _context.Orders.Include(bo => bo.BookOrders).ThenInclude(bo => bo.Book)
                        .FirstOrDefaultAsync(m => m.OrderID == id);

                    List<BookOrder> BooksToRemove = new List<BookOrder>();

                    //NOTE: Thinking that the code to remove items
                    //from the cart according to stock/active books should go here
                    //Subject to change
                    foreach (BookOrder bookOrder in order.BookOrders)
                    {
                        //NOTE: need to check if this logic makes sense
                        //If a book is both out of stock and inactive
                        //would removing it and then requesting to remove it again
                        //make an error? Adding extra code so the remove action
                        //doesn't happen twice

                        //Bool var check to remove order detail
                        bool boolRemoveBook = false;

                        if (bookOrder.Book.CopiesOnHand == 0)
                        {
                            ViewBag.OutOfStockBook = "Sorry! " + bookOrder.Book.Title + "is out of stock and has been removed from your cart.";
                            boolRemoveBook = true;
                        }
                        if (bookOrder.Book.ActiveBook == false)
                        {
                            ViewBag.DiscontinuedBook = "Sorry! " + bookOrder.Book.Title + "has been discontinued and has been removed from your cart.";
                            boolRemoveBook = true;
                        }

                        if (boolRemoveBook == true)
                        {
                            BooksToRemove.Add(bookOrder);
                        }
                    }

                    //Remove books found in list above
                    foreach (BookOrder bo in BooksToRemove)
                    {
                        _context.BookOrders.Remove(bo);
                        _context.SaveChanges();
                    }

                    foreach (BookOrder bo in order.BookOrders)
                    {
                        intTotalBookNum = intTotalBookNum + bo.OrderQuantity;
                    }
                    order.TotalShippingPrice = 3.50m + (1.50m * (intTotalBookNum - 1));

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
        public IActionResult CompletedOrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Finding current logged in user to match user to order id
            string userid = User.Identity.Name;
            User user = _context.Users.FirstOrDefault(us => us.UserName == userid);

            //Finds order (including relational data) of the order matched in the order id
            Order order = _context.Orders
                                .Include(or => or.CreditCard)
                                .Include(or => or.BookOrders).ThenInclude(bo => bo.Book)
                                .Where(or => or.User == user).FirstOrDefault(or => or.OrderID == id);

            string ccNumber = order.CreditCard.CreditCardNumber;

            //To pass credit card view to confirm check out
            string hidden = "************" + ccNumber.Substring(ccNumber.Length - 4);

            ViewBag.ccType = order.CreditCard.CreditType;
            ViewBag.ccHidden = hidden;

            //pass matched order to view
            return View(order);




        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager") == false && order.User.UserName != User.Identity.Name)
            {
                return View("Error", new string[] { "You are not authorized to edit this order!" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        //POST
        //Method to process Add To Order results
        //Has form answers as paramater
        [HttpPost]
        public async Task<IActionResult> AddToOrder(BookOrder bo, int? bookId, int intOrderQuantity)
        {
            if (bookId == null)
            {
                return NotFound();
            }

            if (intOrderQuantity < 1)
            {
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            //Finding book matching book id passed from book details page
            Book book = _context.Books.Find(bookId);

            //Stores book in book order detail
            bo.Book = book;
            bo.OrderQuantity = intOrderQuantity;

            //Finds if user already has an order pending
            //Assigning user to user id
            //get user info
            String id = User.Identity.Name;
            User user = _context.Users.Include(o => o.Orders).ToList().FirstOrDefault(u => u.UserName == id); //NOTE: Relational data

            //If user has an order already
            if (user.Orders.Count != 0)
            {
                if (user.Orders.Exists(o => o.IsPending == true))
                {
                    //TODO

                    //Finds current pending order for this user and stores it in order
                    Order order = _context.Orders.Include(us => us.User).Include(o => o.BookOrders).ThenInclude(o => o.Book).FirstOrDefault(u => u.User.UserName == user.UserName && u.IsPending == true);
                    

                    BookOrder bookOrderToUpdate = new BookOrder();


                    //This is the path for updating a book already in the cart
                    try
                    {
                        //Iterating through list of book orders to add book order quantity instead of
                        //just adding a new book instance
                        foreach (BookOrder bookOrder in order.BookOrders)
                        {
                            if (bookOrder.Book == bo.Book)
                            {

                                bookOrder.OrderQuantity = bookOrder.OrderQuantity + bo.OrderQuantity;
                                bookOrder.Price = bookOrder.Book.BookPrice;
                                bookOrder.ExtendedPrice = bookOrder.Price * bookOrder.OrderQuantity;


                                bookOrderToUpdate = bookOrder;
                                bookOrderToUpdate.Order = order;


                                _context.Update(bookOrderToUpdate);
                                await _context.SaveChangesAsync();

                                ViewBag.AddedOrder = "Your order has been added!";
                                ViewBag.CartMessage = "View your cart below";

                                return RedirectToAction("Details", new { id = bookOrderToUpdate.Order.OrderID });

                            }
                        }

                    }
                    catch
                    {
                        throw;
                    }

                    //This is the path for adding a new book to the cart
                    //Stores matched order in order detail order property
                    bo.Order = order;
                    bo.Price = bo.Book.BookPrice;
                    bo.OrderQuantity = intOrderQuantity;
                    bo.ExtendedPrice = bo.Price * bo.OrderQuantity;

                    _context.Add(bo);
                    await _context.SaveChangesAsync();

                    ViewBag.AddedOrder = "Your order has been added!";
                    ViewBag.CartMessage = "View your cart below";

                    return RedirectToAction("Details", new { id = bo.Order.OrderID });

                }


                //This is the path if user does not have a pending order
                Order neworder = new Order();

                neworder.User = user;

                //Stores newly created order into order detail
                bo.Order = neworder;

                bo.Order.IsPending = true;

                //Stores most recently updated book price into order detail price
                //TODO: Check if we need to do this lol
                bo.Price = bo.Book.BookPrice;

                //Stores order quantity and necessary order details
                bo.OrderQuantity = intOrderQuantity;
                bo.ExtendedPrice = bo.Price * bo.OrderQuantity;
                bo.Order.OrderDate = DateTime.Today;

                if (ModelState.IsValid)
                {
                    _context.Add(neworder);
                    _context.BookOrders.Add(bo);
                    await _context.SaveChangesAsync();

                    ViewBag.AddedOrder = "Your order has been added!";
                    ViewBag.CartMessage = "View your cart below";

                    return RedirectToAction("Details", new { id = bo.Order.OrderID });
                }

                return RedirectToAction("Details", "Books", new { id = bookId });
            }


            //This is the path if this is the user's first order
            Order firstorder = new Order();

            //Assigns user to order
            firstorder.User = user;

            //Stores order to order detail order navigational property
            bo.Order = firstorder;

            //Sets order to is pending so cart can persist
            bo.Order.IsPending = true;

            //Sets order quantity
            bo.OrderQuantity = intOrderQuantity;

            //Stores most recently updated book price into order detail price & other stuff
            bo.Price = bo.Book.BookPrice;
            bo.ExtendedPrice = bo.Price * bo.OrderQuantity;
            bo.Order.OrderDate = DateTime.Today;

            _context.Add(bo);
            await _context.SaveChangesAsync();

            ViewBag.AddedOrder = "Your order has been added!";
            ViewBag.CartMessage = "View your cart below";

            return RedirectToAction("Details", new { id = bo.Order.OrderID });

        }

        //GET method to get order id for order
        public IActionResult RemoveFromOrder(int? id)
        {

            //Storing order into new order instance including relational data
            Order order = _context.Orders.Include(r => r.BookOrders).ThenInclude(r => r.Book).FirstOrDefault(r => r.OrderID == id);

            if (order == null || order.BookOrders.Count == 0)//order is not found
            {
                return RedirectToAction("Details", new { id = id });
            }

            return View(order.BookOrders); //Passes list of orderdetails for matching order id


        }

        //GET: Check out method
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(or => or.User).ThenInclude(or => or.CreditCards).FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        //POST: Check out method processed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id, CheckOutChoice SelectedPaymentMethod, int intSelectedCard, CreditCardType SelectedType, string strCardNumber, string strCouponCode, Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Int32 intTotalBookNum = 0;
                string ccNumber = "";

                try
                {
                    order = _context.Orders
                                .Include(or => or.BookOrders)
                                .Include(u => u.User).ThenInclude(c => c.CreditCards)
                                .FirstOrDefault(o => o.OrderID == id);

                    if (SelectedPaymentMethod == CheckOutChoice.CurrentCard)
                    {
                        //TODO: Finish this
                        //In case user selects incongruent values -> like current card but puts in new card value
                        if (intSelectedCard == 0)
                        {
                            ViewBag.InvalidPayment = "Please choose either a current card or a new card";
                            return RedirectToAction("CheckOut", new { id = order.OrderID });
                        }
                        //Finds credit card with specified creditcard id
                        CreditCard creditCard = _context.CreditCards.Include(u => u.User).FirstOrDefault(c => c.CreditCardID == intSelectedCard);

                        order.CreditCard = creditCard;

                        ccNumber = order.CreditCard.CreditCardNumber;

                    }
                    if (SelectedPaymentMethod == CheckOutChoice.NewCard)
                    {
                        string userid = User.Identity.Name;
                        User user = _context.Users.Include(c => c.CreditCards).ToList().FirstOrDefault(u => u.UserName == userid);

                        if (user.CreditCards.Count() == 3)
                        {
                            ViewBag.CardMax = "You can only store three credit cards in your account! Choose a current card for payment or change cards in account settings.";
                            return RedirectToAction("CheckOut", new { id = order.OrderID });
                        }

                        CreditCard creditCard = new CreditCard();

                        creditCard.CreditType = SelectedType;

                        creditCard.CreditCardNumber = strCardNumber;

                        creditCard.User = user;

                        //for ViewBag
                        ccNumber = strCardNumber;

                        if (ModelState.IsValid)
                        {
                            _context.Add(creditCard);
                            order.CreditCard = creditCard;

                        }
                    }
                    
                    foreach (BookOrder bo in order.BookOrders)
                    {
                        intTotalBookNum = intTotalBookNum + bo.OrderQuantity;
                    }

                    order.TotalShippingPrice = 3.50m + (1.50m * (intTotalBookNum - 1));

                    //To pass credit card view to confirm check out
                    string hidden = "************" + ccNumber.Substring(ccNumber.Length - 4);

                    ViewBag.ccType = order.CreditCard.CreditType;
                    ViewBag.ccHidden = hidden;

                    _context.Update(order);
                    await _context.SaveChangesAsync();

                    return View("ConfirmCheckOut", order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ////TODO: Figure out how the heck coupons work
                //List<Coupon> coupons = new List<Coupon>();

                //try
                //{
                //    Coupon checkcoupon = _context.Coupons.FirstOrDefault(c => c.CouponCode == strCouponCode);

                //    if (ModelState.IsValid)
                //    {

                //    }
                //}
                //catch
                //{
                //    ViewBag.CouponMessage = "This coupon code is invalid! No discount was applied.";
                //    return RedirectToAction("CheckOut", new { id = order.OrderID });
                //}
            }
            return View(order);
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
                Order order = _context.Orders
                                    .Include(or => or.BookOrders).ThenInclude(or => or.Book)
                                    .Include(u => u.User).ThenInclude(c => c.CreditCards)
                                    .FirstOrDefault(o => o.OrderID == id);
                                    

                Book bookUpdate = new Book();

                //Iterates thr
                foreach (BookOrder bo in order.BookOrders)
                {
                    //Matching book object with book for that specific book order detail
                    bookUpdate = _context.Books.Find(bo.Book.BookID);

                    //Subtracts book order quantity from current copies on hand value for this book 
                    bookUpdate.CopiesOnHand = bookUpdate.CopiesOnHand - bo.OrderQuantity;

                    bookUpdate.TimesPurchased = bookUpdate.TimesPurchased + 1; //TODO: Check to see if times purchased means # of copies purchased or purchase instances

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
