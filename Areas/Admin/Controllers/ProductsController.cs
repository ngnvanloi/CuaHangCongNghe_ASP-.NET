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
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        public ActionResult Index(string search = "", int page = 1, string softType = "")
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Product> products = db.Products.Include("Brand").ToList();

            // search
            ViewBag.Search = search;
            products = db.Products.Where(row => row.ProductName.Contains(search)).ToList();
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

        // add new product
        public ActionResult addNewProduct()
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Brand> brands = db.Brands.ToList();
            ViewBag.Brands = brands;
            List<Category> categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        public ActionResult addNewProduct(addNewProductVM avm)
        {
            if (ModelState.IsValid)
            {
                // đổ dữ liệu vào Products
                // 
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";
                int productID;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO Products (ProductName, ProductImage, ProductDescription, Price, Stock, BrandID, CategoryID) OUTPUT INSERTED.ProductID VALUES (@ProductName, @ProductImage, @ProductDescription, @Price, @Stock, @BrandID, @CategoryID)", connection))
                    {
                        command.Parameters.Add("@ProductName", SqlDbType.NVarChar, int.MaxValue ).Value = avm.ProductName;
                        command.Parameters.Add("@ProductImage", SqlDbType.NVarChar, int.MaxValue ).Value = avm.ProductImage;
                        command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar, int.MaxValue ).Value = avm.ProductDescription;
                        command.Parameters.Add("@Price", SqlDbType.Decimal ).Value = avm.Price;
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = avm.Stock;
                        command.Parameters.Add("@BrandID", SqlDbType.Int).Value = avm.BrandID;
                        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = avm.CategoryID;
                        //
                        productID = (int)command.ExecuteScalar();
                    }
                    connection.Close();
                }
                // đổ dữ liệu vào ProductDetails
                // 
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("INSERT INTO ProductDetails (ProductID, ScreenSize, BatteryCapacity, CameraResolution, Ram, Rom) VALUES (@ProductID, @ScreenSize, @BatteryCapacity, @CameraResolution, @Ram, @Rom)", connection))
                    {
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productID;
                        command.Parameters.Add("@ScreenSize", SqlDbType.NVarChar, int.MaxValue).Value = avm.ScreenSize;
                        command.Parameters.Add("@BatteryCapacity", SqlDbType.NVarChar, int.MaxValue).Value = avm.BatteryCapacity;
                        command.Parameters.Add("@CameraResolution", SqlDbType.NVarChar, int.MaxValue).Value = avm.CameraResolution;
                        command.Parameters.Add("@Ram", SqlDbType.Int).Value = avm.Ram;
                        command.Parameters.Add("@Rom", SqlDbType.Int).Value = avm.Rom;

                        // command
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return RedirectToAction("Index", "Products", "Admin");
            }
            else
                return RedirectToAction("addNewProduct", "Products", "Admin");
        }

        // delete products
        public ActionResult deleteProducts(int id)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product product = db.Products.Where(row => row.ProductID == id).FirstOrDefault();
            db.Products.Remove(product);
            ProductDetail productDetail= db.ProductDetails.Where(row => row.ProductID == id).FirstOrDefault();
            db.ProductDetails.Remove(productDetail);
            db.SaveChanges();
            return RedirectToAction("Index", "Products", "Admin");
        }

        // update product
        public ActionResult updateProduct(int id)
        {
            CompanyDBContext db = new CompanyDBContext();
            Product product = db.Products.Where(row => row.ProductID == id).FirstOrDefault();
            ViewBag.Product = product;
            ProductDetail productDetail = db.ProductDetails.Where(row => row.ProductID == id).FirstOrDefault();
            ViewBag.ProductDetail = productDetail;
            List<Brand> brands = db.Brands.ToList();
            ViewBag.Brands = brands;
            List<Category> categories = db.Categories.ToList();
            ViewBag.Categories = categories;            
            return View();
        }
        [HttpPost]
        public ActionResult updateProduct(addNewProductVM avm)
        {
            if(ModelState.IsValid)
            {
                // cập nhật Product
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\CompanyDB.mdf;Integrated Security=True";               
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Products SET ProductName = @ProductName, ProductImage = @ProductImage, ProductDescription = @ProductDescription, Price = @Price, Stock = @Stock, BrandID = @BrandID, CategoryID = @CategoryID WHERE ProductID = @ProductID", connection))
                    {
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = avm.ProductID;
                        command.Parameters.Add("@ProductName", SqlDbType.NVarChar, int.MaxValue).Value = avm.ProductName;
                        command.Parameters.Add("@ProductImage", SqlDbType.NVarChar, int.MaxValue).Value = avm.ProductImage;
                        command.Parameters.Add("@ProductDescription", SqlDbType.NVarChar, int.MaxValue).Value = avm.ProductDescription;
                        command.Parameters.Add("@Price", SqlDbType.Decimal).Value = avm.Price;
                        command.Parameters.Add("@Stock", SqlDbType.Int).Value = avm.Stock;
                        command.Parameters.Add("@BrandID", SqlDbType.Int).Value = avm.BrandID;
                        command.Parameters.Add("@CategoryID", SqlDbType.Int).Value = avm.CategoryID;
                        //
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                // cập nhật Product Detail
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE ProductDetails SET ScreenSize = @ScreenSize, BatteryCapacity = @BatteryCapacity, CameraResolution = @CameraResolution, Ram = @Ram, Rom = @Rom WHERE ProductID = @ProductID", connection))
                    {
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = avm.ProductID;
                        command.Parameters.Add("@ScreenSize", SqlDbType.NVarChar, int.MaxValue).Value = avm.ScreenSize;
                        command.Parameters.Add("@BatteryCapacity", SqlDbType.NVarChar, int.MaxValue).Value = avm.BatteryCapacity;
                        command.Parameters.Add("@CameraResolution", SqlDbType.NVarChar, int.MaxValue).Value = avm.CameraResolution;
                        command.Parameters.Add("@Ram", SqlDbType.Int).Value = avm.Ram;
                        command.Parameters.Add("@Rom", SqlDbType.Int).Value = avm.Rom;
                        //
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return RedirectToAction("Index", "Products", "Admin");
            }
            else
                return RedirectToAction("updateProduct", "Products", "Admin");
        }
    }
}