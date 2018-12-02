using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookOrder
    {

        //BookOrder bridge table properties
        public Int32 BookOrderID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Price { get; set; }

        [Required(ErrorMessage = "You must specify a quantity of books")]
        [Display(Name = "Quantity of books")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public Int32 OrderQuantity { get; set; }

        [Display(Name = "Extended Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal ExtendedPrice { get; set; }

        //Navigation properties
        public Book Book { get; set; }
        public Order Order { get; set; }

    }
}
