using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReview.Models
{
    public class SubCategoryModel
    {
        public string SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string ImageName { get; set; }
    }
}