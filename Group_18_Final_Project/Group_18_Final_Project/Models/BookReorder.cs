using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class BookReorder
    {
        public Int32 BookReorderID { get; set; }
        public String ReorderQuantity { get; set; }
        public List <Book> Books { get; set; }
        public List <Reorder> Reorders { get; set; }

    }
}
