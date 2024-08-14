using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_First___Technology_Products.Identity;
using Code_First___Technology_Products.Models;
using Microsoft.AspNet.Identity;
using Code_First___Technology_Products.ViewModel;
using Microsoft.Owin.Security;
using System.Web.Helpers;
using System.Net.NetworkInformation;
using System.Data.SqlClient;
using System.Data;

namespace Code_First___Technology_Products.Controllers
{
    public class PersonalInformationController : Controller
    {
        // GET: PersonalInformation
        public ActionResult PersonnalInformation()
        {
            // viewbag Cart
            CompanyDBContext cdb = new CompanyDBContext();
            string UserID = User.Identity.GetUserId();
            List<Cart> carts = cdb.Carts.Where(row => row.Id == UserID).ToList();
            ViewBag.Cart = carts;
            // viewbag Order
            List<Order> orders = cdb.Orders.Where(row => row.Id == UserID).ToList();
            ViewBag.Orders = orders;
            if (User.Identity.IsAuthenticated)
            {
                // kết nối với database
                var appDBContext = new AppDbContext();
                // quản lý lưu trữ thông tin chi tiết người dùng
                var userStore = new AppUserStore(appDBContext);
                // quản lý người dùng
                var userManager = new AppUserManager(userStore);
                // thông tin người dùng
                AppUser user = userManager.FindById(User.Identity.GetUserId());
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // update user information
        public ActionResult updateUserInformation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult updateUserInformation(updateUserVM uvm)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                var passHash = Crypto.HashPassword(uvm.Password);
                string userId = User.Identity.GetUserId();
                var user = userManager.FindById(userId);
                // cập nhật thông tin người dùng đăng kí
                user.Email = uvm.Email;
                user.UserName = uvm.Username;
                user.PasswordHash = passHash;
                user.CustomerName = uvm.CustomerName;
                user.Gender = uvm.Gender;
                user.DateOfBirth = uvm.DateOfBirth;
                user.Addres = uvm.Address;
                user.PhoneNumber = uvm.MobilePhone;                
                // update database                
                userManager.UpdateAsync(user);
                // upadate AppUsers                 
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE AppUsers SET UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash, CustomerName = @CustomerName, Gender = @Gender, DateOfBirth = @DateOfBirth, Addres = @Addres, PhoneNumber = @PhoneNumber WHERE Id = @Id", connection))                       
                    {
                        // id
                        command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = user.Id;
                        // infor
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, int.MaxValue).Value = user.Email;
                        command.Parameters.Add("@UserName", SqlDbType.NVarChar, int.MaxValue).Value = user.UserName;
                        command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, int.MaxValue).Value = user.PasswordHash;
                        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, int.MaxValue).Value = user.CustomerName;
                        command.Parameters.Add("@Gender", SqlDbType.NVarChar, int.MaxValue).Value = user.Gender;
                        command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = user.DateOfBirth;
                        command.Parameters.Add("@Addres", SqlDbType.NVarChar, int.MaxValue).Value = user.Addres;
                        command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, int.MaxValue).Value = user.PhoneNumber;
                        // khác
                        //command.Parameters.Add("@EmailConfirmed", SqlDbType.Bit).Value = user.EmailConfirmed;
                        //command.Parameters.Add("@PhoneNumberConfirmed", SqlDbType.Bit).Value = user.PhoneNumberConfirmed;
                        //command.Parameters.Add("@TwoFactorEnabled", SqlDbType.Bit).Value = user.TwoFactorEnabled;
                        //command.Parameters.Add("@LockoutEnabled", SqlDbType.Bit).Value = user.LockoutEnabled;
                        //command.Parameters.Add("@AccessFailedCount", SqlDbType.Int, int.MaxValue).Value = user.AccessFailedCount;
                        //                       
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }
    
    }
}