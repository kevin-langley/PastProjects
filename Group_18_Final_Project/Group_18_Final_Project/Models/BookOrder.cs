using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookOrder
    {
        public Int32 BookOrderID { get; set; }
        public Decimal Price { get; set; }
        public Int32 OrderQuantity { get; set; }
        public List<Book> Books { get; set; }
        public List<Order> Orders { get; set; }

    }
}
