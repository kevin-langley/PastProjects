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
            return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

        ////TODO: Edit the subsequent action methods for project
        ////GET
        ////Method to add products to existing order
        ////Passes in order id
        public IActionResult AddToOrder([Bind("OrderID,OrderDate")] Order order)
        {
            //Finds if user already has an order pending
            //Assigning user to user id
            //get user info
            String id = User.Identity.Name;
            User user = _context.Users.FirstOrDefault(u => u.UserName == id); //TODO: Identity

            //TODO: No squigglies plz
            if (user.Orders.All(o => o.IsPending == true))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(order);
                    _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            //Assigns order type to newly created order detail
            BookOrder bookOrder = new BookOrder() { Orders = order };

            ViewBag.AllProducts = GetAllProducts(); //Populating viewbag for drop down list

            return View("AddToOrder", bookOrder);

        }

        //POST
        //Method to process Add To Order results
        //Has form answers as paramater
        [HttpPost]
        public IActionResult AddToOrder(BookOrder bo, int intSelectedProduct)
        {
            //Finding product matching product in drop down list
            Book book = _context.Books.Find(intSelectedProduct);

            //Stores product in order detail
            bo.Book = book;

            //Finds order in db matching editted order
            Order order = _context.Orders.Find(or.Order.OrderID);

            //Stores order in order detail order
            or.Order = order;

            or.ProductPrice = or.Product.Price;

            or.ExtendedPrice = or.ProductPrice * or.Quantity;

            if (ModelState.IsValid)
            {
                _context.OrderDetails.Add(or);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = or.Order.OrderID });

            }
            //Repopulate Viewbag for modelstate not balid
            ViewBag.AllProducts = GetAllProducts();
            return View(or);

        }

        //GET method to get order id for order
        public IActionResult RemoveFromOrder(int? id)
        {

            //Storing order into new order instance including relational data
            Order order = _context.Orders.Include(r => r.OrderDetails).ThenInclude(r => r.Product).FirstOrDefault(r => r.OrderID == id);

            if (order == null || order.OrderDetails.Count == 0)//registration is not found
            {
                return RedirectToAction("Details", new { id = id });
            }

            return View(order.OrderDetails); //Passes list of orderdetails for matching order id


        }

        //

        //Method to create product list for drop down list
        public SelectList GetAllProducts()
        {
            List<Product> products = _context.Products.ToList();

            SelectList AllProducts = new SelectList(products, "ProductID", "ProductName");

            return AllProducts;


        }
    }
}
