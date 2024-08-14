using Code_First___Technology_Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Code_First___Technology_Products.APIControllers
{
    
    public class ProductController : ApiController
    {
        public List<Product> Get()
        {
            CompanyDBContext db = new CompanyDBContext();
            List<Product> products = db.Products.ToList();
            return products;
        }
    }
}
