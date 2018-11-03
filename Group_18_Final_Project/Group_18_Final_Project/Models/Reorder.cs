using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Reorder
    {
        public Int32 ReorderID { get; set; }
        public User User { get; set; }
        public BookReorder BookReorder { get; set; }
    }
}
