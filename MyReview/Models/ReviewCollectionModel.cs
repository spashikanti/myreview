using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReview.Models
{
    public class ReviewCollectionModel
    {
        public List<ReviewsModel> TopRecentReviews { get; set; }
        public List<ReviewsModel> TrendingReviews { get; set; }
    }
}