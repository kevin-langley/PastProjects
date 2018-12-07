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
                        bo.Price = bo.Book.BookPrice;
                        bo.BookCost = bo.Book.WholesalePrice;
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
                                bookOrder.BookCost = bookOrder.Book.WholesalePrice;
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
                    bo.BookCost = bo.Book.WholesalePrice;
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
                bo.Price = bo.Book.BookPrice;
                bo.BookCost = bo.Book.WholesalePrice;

                //Stores order quantity and necessary order details
                bo.OrderQuantity = intOrderQuantity;
                bo.ExtendedPrice = bo.Price * bo.OrderQuantity;
                bo.Order.OrderDate = DateTime.Today;

                _context.Add(neworder);
                _context.BookOrders.Add(bo);
                await _context.SaveChangesAsync();

                ViewBag.AddedOrder = "Your order has been added!";
                ViewBag.CartMessage = "View your cart below";

                return RedirectToAction("Details", new { id = bo.Order.OrderID });

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
            bo.BookCost = bo.Book.WholesalePrice;
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
                                .Include(or => or.BookOrders).ThenInclude(or => or.Book)
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

                    decimal totalDiscount = 0m;

                    foreach (BookOrder bo in order.BookOrders)
                    {
                        bo.Price = bo.Book.BookPrice;
                        bo.BookCost = bo.Book.WholesalePrice;
                        intTotalBookNum = intTotalBookNum + bo.OrderQuantity;
                        bo.ExtendedPrice = bo.Price * bo.OrderQuantity;

                    }

                    order.TotalShippingPrice = 3.50m + (1.50m * (intTotalBookNum - 1));


                    if (strCouponCode != null && strCouponCode != "")
                    {
                        //Finding coupon matching user's coupon code input
                        Coupon coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == strCouponCode);

                        if (coupon.CouponType == CouponType.FreeShippingForX && order.OrderSubtotal > coupon.CouponValue)
                        {
                            totalDiscount = order.TotalShippingPrice;
                            ViewBag.Discount = "You saved $" + totalDiscount + " on your order!";
                            order.TotalShippingPrice = 0;
                        }

                        if (coupon.CouponType == CouponType.XOffOrder)
                        {
                            foreach (BookOrder border in order.BookOrders)
                            {
                                border.ExtendedPrice = border.ExtendedPrice * (1 - coupon.CouponValue);
                                totalDiscount = totalDiscount + border.ExtendedPrice * coupon.CouponValue;
                            }

                            ViewBag.Discount = "You saved $" + totalDiscount + " on your order!";

                        }
                    }


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
            }
            return View(order);
        }

        //This is the method for placing an order
        //If correctly functioning, this order should update all book stocks by removing quantity
        //This method should also update the shopping cart
        //TODO: Make sure this works correctly
        public async Task<IActionResult> PlaceOrder(int? id, Book PurchasedBook)
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

                ViewBag.ThankYouMessage = "Thanks for placing an order with us!";
                ViewBag.Appreciation = "We appreciate your support.";
                ViewBag.ShippingMessage = "Your order has been shipped. View your order details.";

                //RECOMMENDATIONS
                //Create a variable for a purchased book and create purchased book list
                Order CurrentOrder = _context.Orders.Include(bo => bo.BookOrders).ThenInclude(bo => bo.Book).FirstOrDefault(u => u.OrderID.Equals(id));
                List<BookOrder> PurchasedBookOrderList = _context.BookOrders.Include(o => o.Book).ThenInclude(o => o.Genre).Where(o => o.Order.OrderID == CurrentOrder.OrderID).ToList();

                //Get a purchased book
                foreach (BookOrder bo in PurchasedBookOrderList)
                {
                    PurchasedBook = bo.Book;
                }

                string userid = User.Identity.Name;
                User user = _context.Users.Include(bo => bo.Orders).ThenInclude(o => o.BookOrders).FirstOrDefault(u => u.UserName == userid);

                List<Order> userorder = _context.Orders.Include(bo => bo.BookOrders).ThenInclude(bo => bo.Book).Where(o => o.User == user).ToList();

                

                //Creating new list object of all books in the database
                List<Book> BookList = new List<Book>();

                foreach(Book book in BookList)
                {
                    foreach(Order checkorder in userorder)
                    {
                        if(checkorder.BookOrders.Any(b => b.Book.BookID == book.BookID))
                        {
                            BookList.Remove(book);
                        }
                    }
                }

                //Selecting all book items into a query to processed
                var query = from r in _context.Books
                            select r;

                BookList = query.OrderByDescending(r => r.AverageRating).ToList();

                //Find the recommended books
                string RecommendedBook1 = "book1";
                string RecommendedBook2 = "book2";
                string RecommendedBook3 = "book3";
                string AuthorBook1 = "author1";
                string AuthorBook2 = "author2";
                string AuthorBook3 = "author3";

                


                foreach (Book book in BookList)
                {
                    //First book recommendation
                    if (book.BookID != PurchasedBook.BookID) //Need to make for specific user
                    {
                        if (book.Author == PurchasedBook.Author && book.Genre == PurchasedBook.Genre)
                        {
                            //Sort by rating
                            query = query.Where(r => r.Author == PurchasedBook.Author);
                            query = query.Where(r => r.Genre == PurchasedBook.Genre);

                            Decimal maxRating;
                            maxRating = query.Max(r => r.AverageRating);

                            query = query.Where(r => r.AverageRating == maxRating);

                            List<Book> TopBookList = new List<Book>();
                            TopBookList = query.ToList();

                            //Get a purchased book
                            foreach (Book b in TopBookList)
                            {
                                RecommendedBook1 = b.Title;
                                AuthorBook1 = b.Author;
                            }
                        }
                        else if (book.Genre == PurchasedBook.Genre)
                        {
                            //Sort by rating
                            query = query.Where(r => r.Genre == PurchasedBook.Genre);

                            Decimal maxRating;
                            maxRating = query.Max(r => r.AverageRating);

                            query = query.Where(r => r.AverageRating == maxRating);

                            List<Book> TopBookList = new List<Book>();
                            TopBookList = query.ToList();

                            //Get a purchased book
                            foreach (Book b in TopBookList)
                            {
                                if (RecommendedBook1 == "book1")
                                {
                                    RecommendedBook1 = b.Title;
                                    AuthorBook1 = b.Author;
                                }
                                else if (RecommendedBook2 == "book2" && b.Author != AuthorBook1)
                                {
                                    RecommendedBook2 = b.Title;
                                    AuthorBook2 = b.Author;
                                }
                                else if (RecommendedBook3 == "book3" && b.Author != AuthorBook1 && b.Author != AuthorBook2)
                                {
                                    RecommendedBook3 = b.Title;
                                    AuthorBook3 = b.Author;
                                }
                            }
                        }
                    }
                }

                //If there are not enough books in same genre
                if (RecommendedBook1 == "book1" || RecommendedBook2 == "book2" || RecommendedBook3 == "book3")
                {
                    foreach (Book book in BookList)
                    {

                        if (book.BookID != PurchasedBook.BookID) //Need to make for specific user
                        {
                            //Sort by rating
                            Decimal maxRating;
                            maxRating = query.Max(r => r.AverageRating);

                            query = query.Where(r => r.AverageRating == maxRating);

                            List<Book> TopBookList = new List<Book>();
                            TopBookList = query.ToList();

                            //Get a purchased book
                            foreach (Book b in TopBookList)
                            {
                                if (RecommendedBook1 == "book1")
                                {
                                    RecommendedBook1 = b.Title;
                                    AuthorBook1 = b.Author;
                                }
                                else if (RecommendedBook2 == "book2" && b.Author != AuthorBook1)
                                {
                                    RecommendedBook2 = b.Title;
                                    AuthorBook2 = b.Author;
                                }
                                else if (RecommendedBook3 == "book3" && b.Author != AuthorBook1 && b.Author != AuthorBook2)
                                {
                                    RecommendedBook3 = b.Title;
                                    AuthorBook3 = b.Author;
                                }

                            }
                        }
                    }
                }

                ViewBag.RecommendedBook1 = RecommendedBook1;
                ViewBag.RecommendedBook2 = RecommendedBook2;
                ViewBag.RecommendedBook3 = RecommendedBook3;

                //Send confirmation email
                String emailSubject = "Thank You" + user.FirstName + " for your recent purchase!";
                String emailBody = "You purchased" + order.BookOrders.Count() + "books from us." +
                                   "Your total cost was " + order.OrderTotal + ". Enjoy reading your new books!" +
                                   "We would also like to recommend to you the following books: " +
                                   RecommendedBook1 + ", " + RecommendedBook2 + ", " + RecommendedBook3;
                Utilities.EmailMessaging.SendEmail(user.Email, emailSubject, emailBody);

                return View("OrderConfirmed", order);

            }
            //Sad path :(
            return RedirectToAction("CartDetails");


        }
    }
}
