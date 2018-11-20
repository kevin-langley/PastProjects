using Group_18_Final_Project.dal;
using System;
using System.Linq;

namespace Group_18_Final_Project.Utilities
{
    public static class GenerateBookID
    {
        public static Int32 GetNextBookID(AppDbContext db)
        {
            

            Int32 intMaxBookID; //the current maximum SKU
            Int32 intNextBookID; //the SKU for the next class

            if (db.Books.Count() == 0) //there are no products in the database yet
            {
                intMaxBookID = 5000; //course numbers start at 3001
            }
            else
            {
                intMaxBookID = db.Books.Max(p => p.BookID); //this is the highest number in the database right now
            }

            //add one to the current max to find the next one
            intNextBookID = intMaxBookID + 1;

            //return the value
            return intNextBookID;
        }

    }
}