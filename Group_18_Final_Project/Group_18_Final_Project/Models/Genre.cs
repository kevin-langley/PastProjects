using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Group_18_Final_Project.Models
{
    public class Genre
    {

        //Genre properties
        public Int32 GenreID { get; set; }
        public String GenreName { get; set; }

        //Navigation properties
        public List<Book> Books { get; set; }
    }
}
