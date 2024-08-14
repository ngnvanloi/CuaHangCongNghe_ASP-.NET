using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Code_First___Technology_Products.ViewModel
{
    public class addToOrderVM
    {
        // 
        [Required(ErrorMessage ="Vui lòng nhập ngày !")]
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập ID người dùng !")]
        public string CustomerID { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập Product ID")]
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số hợp lệ")]
        public int Quantity { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập số tiền")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số tiền hợp lệ")]
        public decimal? Price { get; set; }
    }
}