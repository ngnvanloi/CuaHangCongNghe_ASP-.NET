using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_First___Technology_Products.Models;
using Code_First___Technology_Products.Identity;
using System.Web.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Runtime.Remoting.Contexts;
using System.Data.SqlClient;
using System.Data;
using Code_First___Technology_Products.ViewModel;
using Microsoft.Owin.Security;
using Code_First___Technology_Products.Areas.Admin.ViewModel;

namespace Code_First___Technology_Products.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            var appDBContext = new AppDbContext();
            var userStore = new AppUserStore(appDBContext);
            var userManager = new AppUserManager(userStore);
            List<AppUser> users = userStore.Users.ToList();
            
            return View(users);
        }
        // thông tin chi tiết user
        public ActionResult getUserByID(string id)
        {
            var appDBContext = new AppDbContext();
            var userStore = new AppUserStore(appDBContext);
            AppUser user = userStore.Users.Where(row=>row.Id == id).FirstOrDefault();
            return View(user);
        }

        // xóa User
        public ActionResult deleteUserByID(string id)
        {
            // không cho xóa Admin và Manager
            if(id == "50e146ed-5732-4d06-b51a-23f23c9ba73e" || id == "f0a884f3-7ed0-430f-8c41-7e0f3f58a12c")
            {
                return RedirectToAction("Index", "Users");
            }
            // xóa trong AspNetUser
            var appDBContext = new AppDbContext();
            var userStore = new AppUserStore(appDBContext);
            AppUser user = userStore.Users.Where(row => row.Id == id).FirstOrDefault();
            userStore.DeleteAsync(user).Wait();
            userStore.Context.SaveChanges();
            // xóa trong AppUser
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM AppUsers WHERE Id = @Id;", connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = id;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return RedirectToAction("Index", "Users");
        }

        // tạo mới User
        public ActionResult addNewUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addNewUser(RegisterVM rvm)
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
                if (identityResult.Succeeded)
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
                            command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, int.MaxValue).Value = user.CustomerName;
                            command.Parameters.Add("@Gender", SqlDbType.NVarChar, int.MaxValue).Value = user.Gender;
                            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = user.DateOfBirth;
                            command.Parameters.Add("@Addres", SqlDbType.NVarChar, int.MaxValue).Value = user.Addres;
                            command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, int.MaxValue).Value = user.PhoneNumber;
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
                return RedirectToAction("Index", "Users", "Admin");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }


        // cập nhật Users
        public ActionResult updateUserInformation(string id)
        {
            var appDBContext = new AppDbContext();
            var userStore = new AppUserStore(appDBContext);
            AppUser user = userStore.Users.Where(row => row.Id == id).FirstOrDefault();
            updateUserVMAdmin uvm = new updateUserVMAdmin();
            // gán dữ liệu của user cần cập nhật cho đối tượng uvm
            uvm.Id = id;
            uvm.Email = user.Email;
            uvm.Username = user.UserName;
            uvm.CustomerName = user.CustomerName;
            uvm.Gender = user.Gender;
            uvm.DateOfBirth = user.DateOfBirth;
            uvm.Address = user.Addres;
            uvm.MobilePhone = user.PhoneNumber;
            return View(uvm);
        }
        [HttpPost]
        public ActionResult updateUserInformation(updateUserVMAdmin uvm)
        {
            if (ModelState.IsValid)
            {
                var appDBContext = new AppDbContext();
                var userStore = new AppUserStore(appDBContext);
                var userManager = new AppUserManager(userStore);
                // Id người dùng
                string userId = uvm.Id;
                var user = userManager.FindById(userId);
                // cập nhật thông tin người dùng đăng kí
                user.Email = uvm.Email;
                user.UserName = uvm.Username;
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
                    using (var command = new SqlCommand("UPDATE AppUsers SET UserName = @UserName, Email = @Email, CustomerName = @CustomerName, Gender = @Gender, DateOfBirth = @DateOfBirth, Addres = @Addres, PhoneNumber = @PhoneNumber WHERE Id = @Id", connection))
                    {
                        // id
                        command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = user.Id;
                        // infor
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, int.MaxValue).Value = user.Email;
                        command.Parameters.Add("@UserName", SqlDbType.NVarChar, int.MaxValue).Value = user.UserName;                        
                        command.Parameters.Add("@CustomerName", SqlDbType.NVarChar, int.MaxValue).Value = user.CustomerName;
                        command.Parameters.Add("@Gender", SqlDbType.NVarChar, int.MaxValue).Value = user.Gender;
                        command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = user.DateOfBirth;
                        command.Parameters.Add("@Addres", SqlDbType.NVarChar, int.MaxValue).Value = user.Addres;
                        command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, int.MaxValue).Value = user.PhoneNumber;
                                        
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return RedirectToAction("Index", "Users", "Admin");
            }
            else
            {
                ModelState.AddModelError("New Error", "Invalid Data");
                return View();
            }
        }
    }
}
