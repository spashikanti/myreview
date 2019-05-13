using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyReview.Models
{
    public class NewCategoryModel
    {
        //public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name")]
        [Display(Name = "Category Name :")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "Name Length must be Max 100 & Min 3")]
        public string CategoryName { get; set; }

        [Display(Name = "IsActive :")]
        public bool IsActive { get; set; }
    }
}