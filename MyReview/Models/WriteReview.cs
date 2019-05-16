using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReview.Models
{
    public class WriteReview
    {
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string UserId { get; set; }
        public string ProductRate { get; set; }
        public string AvgStarRate { get; set; }
        public string TotalLikes { get; set; }
        public string ReviewTitle { get; set; }

        public string ReviewDescription { get; set; }
        public string IsLikeProduct { get; set; }
        public string IsApproved { get; set; }
        public string FileName { get; set; }

        public string UserEmail { get; set; }
    }
}