using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Book
    {
        //Book properties
        public Int32 BookID { get; set; }

        [Display(Name = "Title")]
        public String Title { get; set; }

        [Display(Name = "Author")]
        public String Author { get; set; }

        [Display(Name = "Unique ID")]
        public Int32 UniqueID { get; set; }

        [Display(Name = "Number of Times Purchased")]
        public Int32 TimesPurchased { get; set; }

        [Display(Name = "Average Rating")]
        [DisplayFormat(DataFormatString = "{0:0.00}")] //Displays average rating to 2 decimals
        public String AverageRating { get; set; }
        
        [Display(Name = "Copies On Hand")]
        public Int32 CopiesOnHand { get; set; }

        [Display(Name = "Book Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal BookPrice { get; set; }

        [Display(Name = "Wholesale Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal WholesalePrice { get; set; }

        [Display(Name = "Active Book")]
        public Boolean ActiveBook { get; set; }

        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Description")]
        public String Description { get; set; }

        //Navigation properties
        public List<BookOrder> BookOrders { get; set; }
        public List<Review> Reviews { get; set; }
        public Genre Genre { get; set; }
    }
}
