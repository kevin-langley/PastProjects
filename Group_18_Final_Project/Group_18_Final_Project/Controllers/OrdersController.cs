﻿using System;
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
        //This is the shopping cart
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //queries db to find user's order
            //includes relational data for book order
            var order = await _context.Orders.Include(bo => bo.BookOrders)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            //NOTE: Thinking that the code to add and remove items
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
                    ViewBag.OutOfStockBook = "Sorry!" + bookOrder.Book.Title + "is out of stock and has been removed from your cart.";
                    boolRemoveBook = true;
                }
                if (bookOrder.Book.ActiveBook == false)
                {
                    ViewBag.DiscontinuedBook = "Sorry!" + bookOrder.Book.Title + "has been discontinued and has been removed from your cart.";                    
                    boolRemoveBook = true;
                }

                if (boolRemoveBook == true)
                {
                    _context.BookOrders.Remove(bookOrder);
                }
            }

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

        //POST
        //Method to process Add To Order results
        //Has form answers as paramater
        [HttpPost]
        public IActionResult AddToOrder(BookOrder bo, int? bookId)
        {
            //Finding book matching book id passed from book details page
            Book book = _context.Books.Find(bookId);

            //Stores product in order detail
            bo.Book = book;

            //Creates new order detail
            BookOrder bookOrder = new BookOrder();

            ////Finds if user already has an order pending
            ////Assigning user to user id
            ////get user info
            //String id = User.Identity.Name;
            //User user = _context.Users.FirstOrDefault(u => u.UserName == id); //TODO: Identity

            ////TODO: Finish this
            ////if user has a pending order
            ////Adds new order detail to current order
            //if (user.Orders.All(o => o.IsPending == true))
            //{
            //    //Finds order in db matching user
            //    Order order = _context.Orders.Find(bo.Order.OrderID);

            //    //Stores order in order detail order
            //    bo.Order = order;

            //    if (ModelState.IsValid)
            //    {
            //        _context.Add(bo);
            //        _context.SaveChangesAsync();

            //        ViewBag.AddedOrder = "Your order has been added!";
            //        ViewBag.CartMessage = "View your cart below";

            //        return RedirectToAction("Details", new { id = bo.Order.OrderID });
            //    }
            //}

            //if user does not have a pending order
            Order neworder = new Order();

            //Stores newly created order into order detail
            bo.Order = neworder;

            //Stores most recently updated book price into order detail price
            //TODO: Check if we need to do this lol
            bo.Price = bo.Book.BookPrice;

            bo.ExtendedPrice = bo.Price;

            if (ModelState.IsValid)
            {
                _context.Add(bo);
                _context.SaveChangesAsync();

                ViewBag.AddedOrder = "Your order has been added!";
                ViewBag.CartMessage = "View your cart below";

                return RedirectToAction("Details" , new { id = bo.Order.OrderID });
            }

            return View(bo);

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

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        //POST: Check out method processed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id, [Bind("OrderID,OrderDate")] Order order)
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


    }
}
