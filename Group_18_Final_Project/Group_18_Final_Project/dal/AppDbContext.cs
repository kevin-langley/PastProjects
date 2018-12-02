using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group_18_Final_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group_18_Final_Project.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books{ get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
        public DbSet<BookReorder> BookReorders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Reorder> Reorders { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
