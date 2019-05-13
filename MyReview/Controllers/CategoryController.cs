using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyReview.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MyReview.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            CategoryModel Category = new CategoryModel();
            Category.CategoryName = PopulateCategory();
            
                
            return View(Category);
        }


        [HttpPost]
        public ActionResult Index(CategoryModel Categories)
        {
            return View();
        }
        public ActionResult AddSubCategory()
        {
            return View();
        }
        private List<SelectListItem> PopulateCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT " +
                    "CategoryName, CategoryId FROM Category";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["CategoryName"].ToString(),
                                Value = sdr["CategoryId"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

    }
}