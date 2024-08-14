using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Code_First___Technology_Products.ViewModel
{
    public class updateUserVM
    {
        [Required(ErrorMessage = "Id cannot be blank !")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Username cannot be blank !")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password cannot be blank !")]
        public string Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword cannot be blank !")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match !")]
        public string ConfirmPassword { get; set; }


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