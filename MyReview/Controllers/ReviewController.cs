﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyReview.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WriteReview()
        {
            return View();
        }
        public ActionResult AllReview()
        {
            return View();
        }
        public ActionResult ReadMore()
        {
            return View();
        }
    }
}