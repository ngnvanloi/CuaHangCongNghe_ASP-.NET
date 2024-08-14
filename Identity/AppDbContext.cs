using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
namespace Code_First___Technology_Products.Identity
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext() : base("MyConnectionString") { }
        //protected override void OnModelCreating(DbModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
        //    builder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
        //    builder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        //}
        //public DbSet<AppUser> AppUserss { get; set; }
    }
}