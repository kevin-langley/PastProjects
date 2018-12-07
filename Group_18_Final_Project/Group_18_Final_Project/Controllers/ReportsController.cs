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
    public enum reportBookOrder
    {
        NewestFirst = 1,
        ProfitAscending = 2,
        ProfitDescending = 3,
        PriceAscending = 4,
        PriceDescending = 5,
        TimesPurchased = 6
    };


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

        //// POST: All Books Sold
        //public IActionResult BooksReport(reportBookOrder SelectedSort)
        //{
                   
        //   //Creating new list object of the repo list
        //    List<Book> SelectedBooks = new List<Book>();

 
        //    //Selecting all repo items into a query to processed
        //    var query = from r in _db.BookOrders
        //                .Include(o => o.Book)       // need to include data from 
        //                .Include(o => o.Order)
        //                select r;

        //    //The following lines of code process the sort by
        //    switch (SelectedSort)
        //    {
        //        case reportBookOrder.NewestFirst:
        //            query = query.OrderByDescending(m => m.OrderDate);

        //            break;

        //        case reportBookOrder.ProfitAscending:
        //            query = query.OrderBy(m => m.PublicationDate); //wrong => how to order by profit margin??????**************

        //            break;

        //        case reportBookOrder.ProfitDescending:
        //            query = query.OrderByDescending(m => m.PublicationDate); //wrong => how to order by profit margin??????**************

        //            break;

        //        case reportBookOrder.PriceAscending:
        //            query = query.OrderBy(m => m.ExtendedPrice);

        //            break;


        //        case reportBookOrder.PriceDescending:
        //            query = query.OrderByDescending(m => m.ExtendedPrice);

        //            break;

        //        case reportBookOrder.TimesPurchased:
        //            query = query.OrderByDescending(m => m.TimesPurchased);

        //            break;
        //    }


        //    //Storing filtered repos to repo list and including language navigational data
        //    SelectedBooks = query.Include(c => c.Order).ToList();  

        //    //ViewBag for Displaying x of y text
        //    ViewBag.SelectedBooks = SelectedBooks.Count();
        //    ViewBag.TotalBooks = _db.Books.Count();

        //    //Redirect to Index View with Selected Repo list to display
        //    return View(SelectedBooks);
        //}

    }
}
