using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;



namespace Group_18_Final_Project.Controllers
{
    public enum reportBooksSold
    {
        NewestFirst = 1,
        ProfitAscending = 2,
        ProfitDescending = 3,
        PriceAscending = 4,
        PriceDescending = 5,
        TimesPurchased = 6
    };

    public enum reportOrdersPlaced
    {
        NewestFirst = 1,
        ProfitAscending = 2,
        ProfitDescending = 3,
        PriceAscending = 4,
        PriceDescending = 5
    }

    public enum reportCustomers
    {
        ProfitAscending = 1,
        ProfitDescending = 2,
        RevenueAscending = 3,
        RevenueDescending = 4
    }

    public enum reportReviews
    {

    }
    public class ReportsController : Controller
    {
        private AppDbContext _db;

        public ReportsController(AppDbContext context)
        {
            _db = context;
        }

        // Home Page Index
        public IActionResult Index()
        {
            return View();
        }
        //------------------------BOOKS REPORT--------------------------------------

        // GET: All Books sold
        public IActionResult SortBooks()
        {
            return View();
        }

        // POST: All Books Sold
        public IActionResult BooksReport(reportBooksSold SelectedSort)
        {

            List<BookOrder> orderDetail = _db.BookOrders.Include(b => b.Book).Include(b => b.Order).ThenInclude(u => u.User).Where(o => o.Order.IsPending == false).ToList();

            //The following lines of code process the sort by
            switch (SelectedSort)
            {
                case reportBooksSold.NewestFirst:
                    //SelectedBooks = _db.Books.Include(m => m.BookOrders).ToList();

                    ViewBag.SelectedBooks = orderDetail.Count();
                    ViewBag.TotalBooks = _db.BookOrders.Count();

                    return View(orderDetail.OrderBy(d => d.Order.OrderDate));

                case reportBooksSold.ProfitAscending:
                    orderDetail = orderDetail.OrderBy(m => m.Profit).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportBooksSold.ProfitDescending:
                    orderDetail = orderDetail.OrderByDescending(m => m.Profit).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportBooksSold.PriceAscending:
                    orderDetail = orderDetail.OrderBy(m => m.Price).ToList();

                    break;


                case reportBooksSold.PriceDescending:
                    orderDetail = orderDetail.OrderByDescending(m => m.Price).ToList();

                    break;

                case reportBooksSold.TimesPurchased:
                    orderDetail = orderDetail.OrderByDescending(m => m.Book.TimesPurchased).ToList();

                    break;
            }

            //ViewBag for Displaying x of y text
            ViewBag.SelectedBooks = orderDetail.Count();
            ViewBag.TotalBooks = _db.BookOrders.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(orderDetail);
        }

        //---------------------------ORDERS REPORT--------------------------------

        // GET: All books sold grouped by orders
        public IActionResult SortOrders()
        {
            return View();
        }

        // POST: All books sold grouped by order
        public IActionResult OrdersReport(reportOrdersPlaced SelectedSort)
        {
            List<Order> orders = _db.Orders.Include(b => b.BookOrders).ThenInclude(b=> b.Book).Include(b => b.User).Where(o => o.IsPending == false).ToList();


            //foreach (Order order in orderList)
            //{
            //    foreach(BookOrder bo in orderDetail)
            //    {
            //        if (order.OrderID == bo.Order.OrderID)
            //        {
                        
            //        }
            //    }
            //}


            //The following lines of code process the sort by
            switch (SelectedSort)
            {
                case reportOrdersPlaced.NewestFirst:
                    //SelectedBooks = _db.Books.Include(m => m.BookOrders).ToList();

                    ViewBag.SelectedBooks = orders.Count();
                    ViewBag.TotalBooks = _db.Orders.Count();

                    return View(orders.OrderBy(d => d.OrderDate));

                case reportOrdersPlaced.ProfitAscending:
                    orders = orders.OrderBy(m => m.BookOrders.Sum(b => b.Profit)).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportOrdersPlaced.ProfitDescending:
                    orders = orders.OrderByDescending(m => m.BookOrders.Sum(b => b.Profit)).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportOrdersPlaced.PriceAscending:
                    orders = orders.OrderBy(m => m.BookOrders.Average(b => b.Price)).ToList();

                    break;


                case reportOrdersPlaced.PriceDescending:
                    orders = orders.OrderByDescending(m => m.BookOrders.Average(b => b.Price)).ToList();

                    break;

            }

            //ViewBag for Displaying x of y text
            ViewBag.SelectedBooks = orders.Count();
            ViewBag.TotalBooks = _db.Orders.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(orders);
        }


        //---------------------------CUSTOMERS REPORT--------------------------------

        // GET: All books sold grouped by orders
        public IActionResult SortCustomers()
        {
            return View();
        }

        //// POST: All books sold grouped by order
        //public IActionResult CustomersReport(reportCustomers SelectedSort)
        //{
        //    //Creating new list object of the repo list
        //    List<User> SelectedCustomers = new List<User>();


        //    List<BookOrder> orderDetail = _db.BookOrders.Include(b => b.Book).Include(b => b.Order).Where(o => o.Order.IsPending == false).ToList();

        //    //create a list of all the books from BookOrder
        //    foreach (BookOrder r in orderDetail)
        //    {
        //        SelectedCustomers.Add(r.Users);
        //    }

        //    //ViewBag for Displaying x of y text
        //    ViewBag.SelectedOrders = SelectedCustomers.Count();
        //    ViewBag.TotalOrders = _db.Users.Count();

        //    //Redirect to Index View with Selected Repo list to display
        //    return View(SelectedCustomers);
        //}


        //------------------------TOTALS REPORT----------------------
        // POST: Totals Report
        public IActionResult TotalsReport()
        {
            Decimal totalProfit = 0;
            Decimal totalCost = 0;
            Decimal totalRevenue = 0;

            List<BookOrder> orderDetail = _db.BookOrders.Where(o => o.Order.IsPending == false).ToList();

            //create a list of all the books from BookOrder
            foreach (BookOrder bo in orderDetail)
            {
                totalProfit = totalProfit + bo.Profit;
                totalCost = totalCost + (bo.OrderQuantity * bo.BookCost);
                totalRevenue = totalRevenue + bo.ExtendedPrice;
            }

            //three viewbags -> para totals!!
            ViewBag.totalProfit = totalProfit;
            ViewBag.totalCost = totalCost;
            ViewBag.totalRevenue = totalRevenue;

            return View();
        }

        // GET: All Books sold
        public IActionResult SortReviews()
        {
            return View();
        }

        // POST: All Books Sold
        public IActionResult ReviewsReport(reportBooksSold SelectedSort)
        {

            List<BookOrder> orderDetail = _db.BookOrders.Include(b => b.Book).Include(b => b.Order).ThenInclude(u => u.User).Where(o => o.Order.IsPending == false).ToList();

            //The following lines of code process the sort by
            switch (SelectedSort)
            {
                case reportBooksSold.NewestFirst:
                    //SelectedBooks = _db.Books.Include(m => m.BookOrders).ToList();

                    ViewBag.SelectedBooks = orderDetail.Count();
                    ViewBag.TotalBooks = _db.BookOrders.Count();

                    return View(orderDetail.OrderBy(d => d.Order.OrderDate));

                case reportBooksSold.ProfitAscending:
                    orderDetail = orderDetail.OrderBy(m => m.Profit).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportBooksSold.ProfitDescending:
                    orderDetail = orderDetail.OrderByDescending(m => m.Profit).ToList(); //wrong => how to order by profit margin??????**************

                    break;

                case reportBooksSold.PriceAscending:
                    orderDetail = orderDetail.OrderBy(m => m.Price).ToList();

                    break;


                case reportBooksSold.PriceDescending:
                    orderDetail = orderDetail.OrderByDescending(m => m.Price).ToList();

                    break;

                case reportBooksSold.TimesPurchased:
                    orderDetail = orderDetail.OrderByDescending(m => m.Book.TimesPurchased).ToList();

                    break;
            }

            //ViewBag for Displaying x of y text
            ViewBag.SelectedBooks = orderDetail.Count();
            ViewBag.TotalBooks = _db.BookOrders.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(orderDetail);
        }



    }

}
