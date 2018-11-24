using Group_18_Final_Project.dal;
using System;
using System.Linq;

namespace Group_18_Final_Project.Utilities
{
    public static class GenerateUserID
    {
        public static Int32 GetNextUserID(AppDbContext db)
        {
            

            Int32 intMaxUserID; //the current maximum SKU
            Int32 intNextUserID; //the SKU for the next class

            if (db.Users.Count() == 0) //there are no products in the database yet
            {
                intMaxUserID = 9000; //course numbers start at 3001
            }
            else
            {
                intMaxUserID = db.Users.Max(p => p.UserID); //this is the highest number in the database right now
            }

            //add one to the current max to find the next one
            intNextUserID = intMaxUserID + 1;

            //return the value
            return intNextUserID;
        }

    }
}