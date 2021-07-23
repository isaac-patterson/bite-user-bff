using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using user_bff.Models;

namespace user_bff.Services
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> opt) : base(opt) { }

        public DbSet<Restaurant> Restaurant { get; set; }

        public DbSet<MenuItem> MenuItem { get; set; }

        public DbSet<MenuItemOption> MenuItemOption { get; set; }

        public DbSet<MenuItemOptionValue> MenuItemOptionValue { get; set; }

        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderItem> OrderItem { get; set; }

        public DbSet<OrderItemOption> OrderItemOption { get; set; }
        
        public DbSet<Coupon> Coupon { get; set; }
    }
}