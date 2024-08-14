using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Code_First___Technology_Products.Identity;

namespace Code_First___Technology_Products.Models
{
    public class CompanyDBContext : DbContext
    {

        public CompanyDBContext() : base("MyConnectionString")
        {
            //this.Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            builder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        //public DbSet<AppUser> AppUsers {get; set;}
        //public DbSet<OrderTotal> OrderTotals { get; set; }
    }
}