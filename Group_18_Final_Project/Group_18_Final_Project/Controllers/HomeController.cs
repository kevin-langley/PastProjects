﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group_18_Final_Project.Controllers
{

    //Enum to process ratings
    //TODO: Enum for ratings (or maybe no enum)

    public class HomeController : Controller
    {
        // Home page index
        public IActionResult Index()
        {
            return View();
        }

        private AppDbContext _db;

        public HomeController(AppDbContext context)
        {
            _db = context;
        }

        public IActionResult Details(int? id)
        {
            if (id == null) //Book id not specified
            {
                return View("Error", new String[] { "Book ID not specified - which repo do you want to view?" });
            }

            Book book = _db.Books.Include(r => r.Genre).FirstOrDefault(r => r.BookID == id);

            if (book == null) //Repo does not exist in database
            {
                return View("Error", new String[] { "Book not found in database" });
            }

            //if code gets this far, all is well
            return View(book);

        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        public ActionResult DisplaySearchResults(String strSearchName, String strSearchAuthor, int SelectedGenre )
        {
            //Creating new list object of the repo list
            List<Book> SelectedBooks = new List<Book>();

            //Selecting all repo items into a query to processed
            var query = from r in _db.Books
                        select r;

            //The following lines of code process the searched repo or user name
            if (strSearchName != null && strSearchName != "")
            {
                query = query.Where(r => r.Title.Contains(strSearchName) || r.Author.Contains(strSearchName));
            }

            //The following lines of code process the searched description terms
            if (strSearchAuthor != null && strSearchAuthor != "")
            {
                query = query.Where(r => r.Description.Contains(strSearchAuthor));
            }

            //The following lines of code process the language selected from the drop down menu
            if(SelectedGenre != 0)
            {
                query = query.Where(r => r.Genre.GenreID == SelectedGenre);

            }

            //Storing filtered repos to repo list and including language navigational data
            SelectedBooks = query.Include(r => r.Genre).ToList();

            //ViewBag for Displaying x of y text
            ViewBag.SelectedBooks = SelectedBooks.Count();
            ViewBag.TotalBooks = _db.Books.Count();

            //Redirect to Index View with Selected Repo list to display
            return View(SelectedBooks);
        }

        //The following method creates new language option for user that does not want to choose a specific language
        public SelectList GetAllGenres()
        {
            List<Genre> Genres = _db.Genres.ToList();

            //Adds record for all languages
            Genre SelectNone = new Genre() { GenreID = 0, GenreName = "All Genres" };
            Genres.Add(SelectNone);

            //Converts list to select list
            SelectList AllGenres = new SelectList(Genres.OrderBy(l => l.GenreID),
                                                     "GenreID", "GenreName");

            //return the select list
            return AllGenres;
        }
    }
}
