using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyReview.Models
{

    public class CategoryModel
    {
        public List<SelectListItem> CategoryName { get; set; }
        public int[] CategoryId { get; set; }
    }

}