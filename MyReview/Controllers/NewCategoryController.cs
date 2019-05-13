using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyReview.Models;

namespace MyReview.Controllers
{
    public class NewCategoryController : Controller
    {
        // GET: NewCategory
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(NewCategoryModel nCM)
        {
            if (!ModelState.IsValid)
            {
                
            }
            else
            {

            }
            return View();
        }
    }
}