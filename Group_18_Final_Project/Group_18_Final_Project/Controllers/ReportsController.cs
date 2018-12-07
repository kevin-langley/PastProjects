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
            //Creating new list object of the repo list
            List<Book> SelectedBooks = new List<Book>();


            List<BookOrder> orderDetail = _db.BookOrders.Include(b => b.Book).Include(b => b.Order).Where(o => o.Order.IsPending == false).ToList();

            //create a list of all the books from BookOrder
            foreach (BookOrder bo in orderDetail)
            {
                SelectedBooks.Add(bo.Book);
            }

            ////The following lines of code process the sort by
            //switch (SelectedSort)
            //{
            //    case reportBookOrder.NewestFirst:
            //        //SelectedBooks = _db.Books.Include(m => m.BookOrders).ToList();

            //        ViewBag.SelectedBooks = SelectedBooks.Count();
            //        ViewBag.TotalBooks = _db.Books.Count();

            //        return View(SelectedBooks.OrderBy(d => OrderDate));

            //    case reportBookOrder.ProfitAscending:
            //        SelectedBooks = SelectedBooks.OrderBy(m => m.PublicationDate).ToList(); //wrong => how to order by profit margin??????**************

            //        break;

            //    case reportBookOrder.ProfitDescending:
            //        SelectedBooks = SelectedBooks.OrderByDescending(m => m.PublicationDate).ToList(); //wrong => how to order by profit margin??????**************

            //        break;

            //    case reportBookOrder.PriceAscending:
            //        SelectedBooks = SelectedBooks.OrderBy(m => m.ExtendedPrice);

            //        break;


            //    case reportBookOrder.PriceDescending:
            //        SelectedBooks = SelectedBooks.OrderByDescending(m => m.ExtendedPrice);

            //        break;

            //    case reportBookOrder.TimesPurchased:
            //        SelectedBooks = SelectedBooks.OrderByDescending(m => m.TimesPurchased).ToList();

            //        break;
            //}

            //ViewBag for Displaying x of y text
            ViewBag.SelectedBooks = SelectedBooks.Count();
            ViewBag.TotalBooks = _db.Books.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(SelectedBooks);
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
            //Creating new list object of the repo list
            List<Order> SelectedOrders = new List<Order>();


            List<BookOrder> orderDetail = _db.BookOrders.Include(b => b.Book).Include(b => b.Order).Where(o => o.Order.IsPending == false).ToList();

            //create a list of all the books from BookOrder
            foreach (BookOrder r in orderDetail)
            {
                SelectedOrders.Add(r.Order);
            }

            //ViewBag for Displaying x of y text
            ViewBag.SelectedOrders = SelectedOrders.Count();
            ViewBag.TotalOrders = _db.Orders.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(SelectedOrders);

        }
    }
}
