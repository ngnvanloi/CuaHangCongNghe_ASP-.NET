using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_First___Technology_Products.ViewModel;
using Code_First___Technology_Products.Identity;
using Code_First___Technology_Products.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Helpers;
using Microsoft.Owin.BuilderProperties;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;

namespace Code_First___Technology_Products.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM rvm)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                var passHash = Crypto.HashPassword(rvm.Password);

                // lưu thông tin người dùng đăng kí
                var user = new AppUser()
                {
                    Email = rvm.Email,
                    UserName = rvm.Username,
                    PasswordHash = passHash,
                    CustomerName = rvm.CustomerName,
                    Gender = rvm.Gender,
                    DateOfBirth = rvm.DateOfBirth,
                    Addres = rvm.Address,
                    PhoneNumber = rvm.MobilePhone,
                };

                // tạo người dùng từ user
                IdentityResult identityResult = userManager.Create(user);
                if(identityResult.Succeeded)
                {
                    // thêm role customer vào user mới đăng kí
                    userManager.AddToRole(user.Id, "Customer");
                    // đăng nhập cho user mới
                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);

                    // đổ dữ liệu vào AppUser                    
                    //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\CODE IN UNIVERSITY\\Technology Product Code First\\Code First _ Technology Products\\Code First _ Technology Products\\App_Data\\CompanyDB.mdf\";Integrated Security=True";
                    string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("INSERT INTO AppUsers (Id, Email, UserName,PasswordHash,CustomerName,Gender,DateOfBirth,Addres,PhoneNumber, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount) VALUES (@Id, @Email, @UserName, @PasswordHash, @CustomerName, @Gender, @DateOfBirth, @Addres, @PhoneNumber, @EmailConfirmed, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled, @AccessFailedCount)", connection))
                        {
                            command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = user.Id;
                            command.Parameters.Add("@Email", SqlDbType.NVarChar, int.MaxValue).Value = user.Email;
                            command.Parameters.Add("@UserName", SqlDbType.NVarChar, int.MaxValue).Value = user.UserName;
                            command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, int.MaxValue).Value = user.PasswordHash;
                            command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, int.MaxValue).Value = user.CustomerName ;
                            command.Parameters.Add("@Gender", SqlDbType.NVarChar, int.MaxValue).Value = user.Gender ;
                            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = user.DateOfBirth ;
                            command.Parameters.Add("@Addres", SqlDbType.NVarChar, int.MaxValue).Value = user.Addres ;
                            command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, int.MaxValue).Value = user.PhoneNumber ;
                            // khác
                            command.Parameters.Add("@EmailConfirmed", SqlDbType.Bit).Value = user.EmailConfirmed;
                            command.Parameters.Add("@PhoneNumberConfirmed", SqlDbType.Bit).Value = user.PhoneNumberConfirmed;
                            command.Parameters.Add("@TwoFactorEnabled", SqlDbType.Bit).Value = user.TwoFactorEnabled;
                            command.Parameters.Add("@LockoutEnabled", SqlDbType.Bit).Value = user.LockoutEnabled;
                            command.Parameters.Add("@AccessFailedCount", SqlDbType.Int, int.MaxValue).Value = user.AccessFailedCount;
                            //
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM lvm)
        {
            if(ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                var user = userManager.Find(lvm.Username, lvm.Password);
                if(user != null)
                {
                    var authenManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenManager.SignIn(new AuthenticationProperties(), userIdentity);
                    if(userManager.IsInRole(user.Id, "Admin"))
                    {
                        return RedirectToAction("Index", "Home", new {area = "Admin"});
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("loginfail", "Invalid Username and Password");
                return View();
            }
            
        }
      
        public ActionResult Logout()
        {
            var authenManager = HttpContext.GetOwinContext().Authentication;
            authenManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}