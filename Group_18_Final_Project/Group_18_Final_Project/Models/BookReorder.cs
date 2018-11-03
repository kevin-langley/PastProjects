using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookReorder
    {
        public Int32 BookReorderID { get; set; }
        public String Quantity { get; set; }
        public Book Book { get; set; }
        public Reorder Reorder { get; set; }

    }
}
