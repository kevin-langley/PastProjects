using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookOrder
    {

        //BookOrder bridge table properties
        public Int32 BookOrderID { get; set; }
        public Decimal Price { get; set; }
        public Int32 OrderQuantity { get; set; }

        //Navigation properties
        public Book Book { get; set; }
        public Order Order { get; set; }

    }
}
