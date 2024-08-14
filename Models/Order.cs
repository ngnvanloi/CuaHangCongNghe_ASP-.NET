using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Code_First___Technology_Products.Identity;

namespace Code_First___Technology_Products.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal Total { get; set; }


        // Id là của AppUser.Id
        //[ForeignKey("Id")]
        public string Id { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}