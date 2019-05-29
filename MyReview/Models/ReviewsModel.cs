using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReview.Models
{
    public class ReviewsModel
    {
        public int ReviewId { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ProductRate { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDescription { get; set; }
        public string PublishedSince { get; set; }
        public int UserId { get; set; }
        public decimal StarRate { get; set; }
        public int TotalLikes { get; set; }
        public string UserPhoto { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CategoryId { get; set; }
        public int NumOfReviews { get; set; }
        public bool IsTrending { get; set; }
    }
}