using MyReview.Models;
using System;
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
        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            //Note : you can bind same list from database  
            List<SubCategoryModel> ObjList = new List<SubCategoryModel>()
            {

                new SubCategoryModel {SubCategoryName="Latur",Description="Latur City is good" },
                new SubCategoryModel {SubCategoryName="Mumbai",Description="Mumbai city is huge" },
                new SubCategoryModel {SubCategoryName="Pune" ,Description="Pune city is cool" },
                new SubCategoryModel {SubCategoryName="Delhi",Description="Delhi city is crowdy" },
                new SubCategoryModel {SubCategoryName="Dehradun",Description="Dehradun city is pleasant" },
                new SubCategoryModel {SubCategoryName="Noida",Description="Noida city is creepy" },
                new SubCategoryModel {SubCategoryName="New Delhi",Description="New Delhi city is capital" }

        };
            //Searching records from list using LINQ query  
            var CityList = (from N in ObjList
                            where N.SubCategoryName.StartsWith(Prefix)
                            select new { N.SubCategoryName });
            return Json(CityList, JsonRequestBehavior.AllowGet);
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