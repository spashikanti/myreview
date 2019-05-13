using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyReview.Models
{
    public class UserModel
    {
        [Required(ErrorMessage ="Enter Name")]
        [Display(Name = "Enter Name :")]
        [StringLength(maximumLength:125,MinimumLength =3,ErrorMessage ="Name Length must be Max 125 & Min 3")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Phone No.")]
        [Display(Name = "Enter Phone :")]
        [StringLength(maximumLength: 20, MinimumLength = 10 , ErrorMessage = "Your name Length must be Max 20 & Min 10")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Enter the Email")]
        [Display(Name = "Email Address :")]
        [EmailAddress(ErrorMessage = "Provide valid Email Address")]
        [StringLength(maximumLength: 150, MinimumLength = 10, ErrorMessage = "Your Email Address Length must be Max 150 & Min 10")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the Pasword")]
        [Display(Name = "Password :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ReEnter the Pasword")]
        [Display(Name = "Re-Password :")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string APassword { get; set; }

        [Required(ErrorMessage = "Upload Profile Image")]
        [Display(Name = "Profile Image :")]
        public string Photo { get; set; }

    }
}