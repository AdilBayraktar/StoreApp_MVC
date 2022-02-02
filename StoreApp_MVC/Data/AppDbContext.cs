using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreApp_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp_MVC.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<StoreApp_MVC.Models.Type> Type { get; set; }
        public DbSet<StoreApp_MVC.Models.Product> Product { get; set; }
        public DbSet<StoreApp_MVC.Models.AppUser> AppUsers { get; set; }
    }
}
