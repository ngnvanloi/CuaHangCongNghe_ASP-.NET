using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Code_First___Technology_Products.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Code_First___Technology_Products.Identity
{
    public class AppUser : IdentityUser
    {
        // tạo bảng thông tin người dùng đăng nhập
        // giống bảng Custumer
        // Nhưng vì lớp AppUser kế thừa từ IdentityUser nên đã có sẵn các cột như ID, Email, ...
       
        public string CustomerName { get; set; }     
        public string Gender { get; set; }
        public string Addres { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        //public virtual ICollection<OrderTotal> OrderTotals { get; set; }
    }
}