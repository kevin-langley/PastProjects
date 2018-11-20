using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group_18_Final_Project.dal;
using Group_18_Final_Project.Models;
using Group_18_Final_Project.Utilities;

namespace Group_18_Final_Project.Seeding
{
    public class SeedBooks
    {
        public static void SeedAllBooks(AppDbContext db)
        {
            if (db.Books.Count() == 300)
            {
                throw new NotSupportedException("The database already contains all 300 books!");
            }

            Int32 intBooksAdded = 0;
            String bookName = "Begin"; //helps to keep track of error on books
            List<Book> Books = new List<Book>();

            try
            {
                Book b1 = new Book();
                b1.BookID = GenerateBookID.GetNextBookID(db);
                b1.Title = "Crescent Dawn";
                b1.Author = "Clive Cussler and Dirk Cussler";
                b1.UniqueID = 789075;
                b1.CopiesOnHand = 2;
                b1.BookPrice = 27.95m;
                b1.WholesalePrice = 20.12m;
                b1.PublicationDate = new DateTime(2010, 11, 20);


                //loop through repos
                foreach (Book b in Books)
                {
                    //set title to help debug
                    bookName = b.Title;

                    //see if title exists in database
                    Book dbBook = db.Books.FirstOrDefault(r => r.Title == b.Title);

                    if (dbBook == null) //repository does not exist in database
                    {
                        db.Books.Add(b);
                        db.SaveChanges();
                        intBooksAdded += 1;
                    }
                    else
                    {
                        dbBook.BookID = b.BookID;
                        dbBook.PublicationDate = b.PublicationDate;
                        dbBook.Title = b.Title;
                        dbBook.Author = b.Author;
                        dbBook.UniqueID = b.UniqueID;
                        dbBook.CopiesOnHand = b.CopiesOnHand;
                        dbBook.BookPrice = b.BookPrice;
                        dbBook.WholesalePrice = b.WholesalePrice;
                        //might need to add in the rest of the date here idk
                        db.Update(dbBook);
                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                String msg = "Boooks added:" + intBooksAdded + "; Error on " + bookName;
                throw new InvalidOperationException(msg);
            }

        }



        

        
    }
}
