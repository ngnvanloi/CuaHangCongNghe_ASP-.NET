using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Code_First___Technology_Products.Areas.Admin.ViewModel
{
    public class updateUserVMAdmin
    {
        [Required(ErrorMessage = "Id cannot be blank !")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Username cannot be blank !")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email cannot be blank !")]
        [EmailAddress(ErrorMessage = "Invalid Email !")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Numberphone cannot be blank !")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "NumberPhone is invalid !")]
        public string MobilePhone { get; set; }


        [Required(ErrorMessage = "Your name cannot be blank !")]
        public string CustomerName { get; set; }


        public DateTime? DateOfBirth { get; set; }


        public string Gender { get; set; }
        [Required(ErrorMessage = "Address cannot be blank !")]


        public string Address { get; set; }
    }
}