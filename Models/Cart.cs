using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Code_First___Technology_Products.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Code_First___Technology_Products.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }

        // Id là của AppUser.Id
        //[ForeignKey("Id")]
        public string Id { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}