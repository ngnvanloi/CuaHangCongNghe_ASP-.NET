using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Code_First___Technology_Products.Areas.Admin.ViewModel
{
    public class addNewProductVM
    {
        // thông tin tổng quát
        public string ProductID { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập URL hình ảnh")]
        public string ProductImage { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số tiền")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng nhập số tiền hợp lệ")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tồn kho")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập một số nguyên hợp lệ.")]
        public int Stock { get; set; }
        public int BrandID { get; set; }
        public int CategoryID { get; set; }


        // thông tin chi tiết
        [Required(ErrorMessage = "Vui lòng nhập Screen Size")]

        public string ScreenSize { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập BatteryCapacity")]

        public string BatteryCapacity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập CameraResolution")]

        public string CameraResolution { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập RAM")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập một số nguyên hợp lệ.")]
        public int Ram { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ROM")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập một số nguyên hợp lệ.")]
        public int Rom { get; set; }
    }
}