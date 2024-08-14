using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_First___Technology_Products.Models;
using Code_First___Technology_Products.Identity;
using Microsoft.AspNet.Identity;
using Code_First___Technology_Products.ViewModel;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Security.Cryptography;
using Code_First___Technology_Products.Areas.Admin.ViewModel;
namespace Code_First___Technology_Products.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Admin/Orders
        public ActionResult Index()
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Order> orders = db.Orders.Include("AppUser").ToList();
            return View(orders);
        }
    }
}