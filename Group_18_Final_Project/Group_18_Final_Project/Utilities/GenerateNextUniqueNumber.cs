using Group_18_Final_Project.DAL;
using System;
using System.Linq;


namespace Group_18_Final_Project.Utilities
{
    public static class GenerateNextUniqueNumber
    {
        public static Int32 GetNextUniqueNumber(AppDbContext db)
        {
            Int32 intMaxUniqueNumber; //the current maximum course number
            Int32 intNextUniqueNumber; //the course number for the next class

            if (db.Books.Count() == 0) //there are no orders in the database yet
            {
                intMaxUniqueNumber = 789300; //registration numbers start at 101
            }
            else
            {
                intMaxUniqueNumber = db.Books.Max(c => c.UniqueID); //this is the highest number in the database right now
            }

            //add one to the current max to find the next one
            intNextUniqueNumber = intMaxUniqueNumber + 1;

            //return the value
            return intNextUniqueNumber;
        }

    }
}