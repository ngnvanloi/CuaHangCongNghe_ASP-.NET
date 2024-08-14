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
namespace Code_First___Technology_Products.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home

        // hiển thị trang chủ
        public ActionResult Index()
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Product> products = db.Products.ToList();
            return View(products);
        }
        
        // hiển thị random danh sách sản phẩm
        public ActionResult listRandomProduct(string search = "", int page = 1, string softType = "", int BrandID = 0)
        {
            CompanyDBContext db = new CompanyDBContext();
            // danh sách brand
            List<Brand> brands = db.Brands.ToList();
            ViewBag.Brand = brands;
            ViewBag.BrandID = BrandID;
            List<Product> products = db.Products.ToList();
            // search
            ViewBag.Search = search;
            products = db.Products.Where(row => row.ProductName.Contains(search)).ToList();
            // tìm kiếm theo hãng
            if(BrandID != 0)
            {
                ViewBag.BrandID = BrandID;
                products = db.Products.Where(row => row.Brand.BrandID == BrandID).ToList();
            }
            // sắp xếp
            ViewBag.SoftType = softType;
            if (softType == "priceIncrease")
            {
                products = products.OrderBy(row => row.Price).ToList();
            }
            else if (softType == "priceDecrease")
            {
                products = products.OrderByDescending(row => row.Price).ToList();
            }
            // paging
            int noOfRecordPerPage = 10;
            int noOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(noOfRecordPerPage)));
            int noOfRecordToSkip = (page - 1) * noOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = noOfPages;
            products = products.Skip(noOfRecordToSkip).Take(noOfRecordPerPage).ToList();
            return View(products);
        }

        // lấy thông tin sản phẩm qua ID
        public ActionResult getProductByID(int id)
        {
            CompanyDBContext db = new CompanyDBContext();
            ProductDetail productDetail = db.ProductDetails.Where(row => row.ProductID == id).FirstOrDefault();
            Product product = db.Products.Where(row => row.ProductID == id).FirstOrDefault();
            Brand brand = db.Brands.Where(row => row.BrandID == product.BrandID).FirstOrDefault();
            string customerID = User.Identity.GetUserId();
            
            ViewBag.CustomerID = customerID;
            ViewBag.Brand = brand;
            ViewBag.ProductTotal = product;
            return View(productDetail);
        }
        
        // thêm giỏ hàng
        public ActionResult addToCart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addToCart(Cart cart)
        {
            CompanyDBContext db = new CompanyDBContext();
            db.Carts.Add(cart);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // orders
        public ActionResult addToOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addToOrder(addToOrderVM avm)
        {  
            if(ModelState.IsValid)
            {
                // lưu trữ dữ liệu
                DateTime? orderDate = avm.OrderDate;
                decimal? total = avm.TotalPrice;
                string customerID = avm.CustomerID;
                int productID = avm.ProductID;
                int quantity = avm.Quantity;
                decimal? price = avm.Price;
                decimal? totalPrice = quantity * price;
                // đổ dữ liệu vào Orders
                // 
                //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\CODE IN UNIVERSITY\\Technology Product Code First\\Code First _ Technology Products\\Code First _ Technology Products\\App_Data\\CompanyDB.mdf\";Integrated Security=True";
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";
                int orderID;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO Orders (OrderDate, Total, Id) OUTPUT INSERTED.OrderID VALUES (@OrderDate, @Total, @Id)", connection))
                    {
                        command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderDate;
                        command.Parameters.Add("@Total", SqlDbType.Decimal).Value = totalPrice;
                        command.Parameters.Add("@Id", SqlDbType.NVarChar, 128).Value = customerID;
                        //
                        orderID = (int)command.ExecuteScalar();
                    }
                    connection.Close();
                }

                // đổ dữ liệu vào OrderDetails
                // 
                //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"D:\\CODE IN UNIVERSITY\\Technology Product Code First\\Code First _ Technology Products\\Code First _ Technology Products\\App_Data\\CompanyDB.mdf\";Integrated Security=True";               
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) VALUES (@OrderID, @ProductID, @Quantity, @Price)", connection))
                    {
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderID;
                        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;
                        command.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;

                        //
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // xóa giỏ hàng
        public ActionResult deleteCart(int id)
        {
            CompanyDBContext db = new CompanyDBContext();
            Cart cart = db.Carts.Where(row => row.CartID == id).FirstOrDefault();
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // API sản phẩm
        public ActionResult apiListProductGET()
        {
            return View();
        }
    }
}