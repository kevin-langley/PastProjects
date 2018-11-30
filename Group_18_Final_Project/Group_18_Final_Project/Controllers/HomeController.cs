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


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group_18_Final_Project.Controllers
{

    //Enum to process ratings
    //TODO: Enum for ratings (or maybe no enum)

    //Enum to process sort order
    public enum SortOrder
    {
        Title = 1,
        Author = 2,
        MostPopular = 3,
        NewestFirst = 4,
        OldestFirst = 5,
        HighestRated = 6
    };

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

        public ActionResult DisplaySearchResults(String strSearchName, String strSearchAuthor, Int32 intSearchUniqueID, int SelectedGenre, SortOrder SelectedSort, Boolean SelectedStock )
        {
            //Creating new list object of the repo list
            List<Book> SelectedBooks = new List<Book>();

            String strUniqueID;

            try
            {
                strUniqueID = intSearchUniqueID.ToString();
            }
            catch
            {
                ViewBag.AllGenres = GetAllGenres();
                return RedirectToAction("DetailedSearch");
            }

            //Selecting all repo items into a query to processed
            var query = from r in _db.Books
                        select r;

            //The following lines of code process the searched book name
            if (strSearchName != null && strSearchName != "")
            {
                query = query.Where(r => r.Title.Contains(strSearchName) || r.Author.Contains(strSearchName));
            }

            //The following lines of code process the searched author
            if (strSearchAuthor != null && strSearchAuthor != "")
            {
                query = query.Where(r => r.Author.Contains(strSearchAuthor));
            }

            //The following lines of code process the searched unique number
            if (strUniqueID != null || strUniqueID != "")
            {
                query = query.Where(r => r.UniqueID.ToString().Contains(strUniqueID));
            }

            //The following lines of code process the genre selected from the drop down menu
            if (SelectedGenre != 0)
            {
                query = query.Where(r => r.Genre.GenreID == SelectedGenre);

            }

            //The following lines of code process the sort by
            switch (SelectedSort)
            {
                case SortOrder.Title:
                    query = query.OrderBy(o => o.Title);

                    break;

                case SortOrder.Author:
                    query = query.OrderBy(o => o.Author);

                    break;

                case SortOrder.MostPopular:
                    query = query.OrderByDescending(o => o.TimesPurchased);

                    break;

                case SortOrder.NewestFirst:
                    query = query.OrderByDescending(o => o.PublicationDate); //Check to see if order by newest date goes from newest to oldest

                    break;

                case SortOrder.OldestFirst:
                    query = query.OrderBy(o => o.PublicationDate);

                    break;

                case SortOrder.HighestRated:
                    query = query.OrderBy(o => o.AverageRating);

                    break;

            }

            if (SelectedStock == true)
            {
                query = query.Where(o => o.CopiesOnHand > 0);
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
