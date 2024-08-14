using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Code_First___Technology_Products.Models
{
    public class ProductDetail
    {
        [Key]
        public int DetailID { get; set; }
        public int ProductID { get; set; }
        public string ScreenSize { get; set; }
        public string BatteryCapacity { get; set; }
        public string CameraResolution { get; set; }
        public int Ram { get; set; }
        public int Rom { get; set; }

        public virtual Product Product { get; set; }
    }
}