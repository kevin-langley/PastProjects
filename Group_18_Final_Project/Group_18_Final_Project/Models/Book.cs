using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Book
    {
        public Int32 BookID { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }
        public Int32 UniqueID { get; set; }
        public Int32 TimesPurchased { get; set; }
        public Int32 AverageRating { get; set; }
        public Int32 CopiesOnHand { get; set; }
        public Decimal BookPrice { get; set; }
        public Decimal WholesalePrice { get; set; }
        public Boolean ActiveBook { get; set; }
        public DateTime PublicationDate { get; set; }
        public BookOrder BookOrder { get; set; }
        public List<Review> Reviews { get; set; }
        public Genre Genre { get; set; }
    }
}
