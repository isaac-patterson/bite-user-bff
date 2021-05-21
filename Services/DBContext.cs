using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using veni_bff.Models;

namespace veni_bff.Services
{
    public class DBContext : DbContext
   {
      string DB_CONNECTION_STRING = "";
      
      public DBContext(IOptions<Parameters> options)
      {
            Console.WriteLine("DB Connection Context - " + options.Value.AuroraConnectionString);
            DB_CONNECTION_STRING = options.Value.AuroraConnectionString;
            //DB_CONNECTION_STRING = "Server=LAPTOP-9O09LJJ9;Database=veni;Uid=isaac;Pwd=password";
      }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
            optionsBuilder.UseMySQL(DB_CONNECTION_STRING);
            base.OnConfiguring(optionsBuilder);
      }
      public DbSet<ToDo> ToDos { get; set; }

      public DbSet<Restaurant> Restaurant { get; set; }

      public DbSet<MenuItem> MenuItem { get; set; }

      public DbSet<MenuItemOption> MenuItemOption { get; set; }

      public DbSet<MenuItemOptionValue> MenuItemOptionValue { get; set; }


    }
}