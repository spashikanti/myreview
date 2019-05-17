using MyReview.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        public JsonResult Index(string keyword)
        {
            //Note : you can bind same list from database  
            List<SubCategoryModel> ObjList = GetSearchItems(keyword);
                //new List<SubCategoryModel>()
                //    {

            //        new SubCategoryModel {SubCategoryName="Latur",Description="Latur City is good" },
            //        new SubCategoryModel {SubCategoryName="Mumbai",Description="Mumbai city is huge" },
            //        new SubCategoryModel {SubCategoryName="Pune" ,Description="Pune city is cool" },
            //        new SubCategoryModel {SubCategoryName="Delhi",Description="Delhi city is crowdy" },
            //        new SubCategoryModel {SubCategoryName="Dehradun",Description="Dehradun city is pleasant" },
            //        new SubCategoryModel {SubCategoryName="Noida",Description="Noida city is creepy" },
            //        new SubCategoryModel {SubCategoryName="New Delhi",Description="New Delhi city is capital" }

            //};
            //Searching records from list using LINQ query  
            //var CityList = (from N in ObjList
            //                where N.SubCategoryName.StartsWith(Prefix)
            //                select new { N.SubCategoryName });
            return Json(ObjList, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult WriteReview()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddReview(FormCollection f)
        {
            InsertReviewDetails("1", "1", 1, f["reviewtitle"].ToString(), f["comment"].ToString(), f["filename"].ToString(), "xxx@gmail.com");
            return View("WriteReview");
        }
        public ActionResult AllReview()
        {
            return View();
        }
        public ActionResult ReadMore()
        {
            return View();
        }

        private List<SubCategoryModel> GetSearchItems(string keyword)
        {
            List<SubCategoryModel> items = new List<SubCategoryModel>();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SearchProducts";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter prodname = new SqlParameter("@prodname", keyword);
                    cmd.Parameters.Add(prodname);

                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SubCategoryModel
                            {
                                SubCategoryId = sdr["SubCategoryId"].ToString(),
                                SubCategoryName = sdr["SubCategoryName"].ToString(),
                                Description = sdr["Description"].ToString(),
                                ImageName = sdr["ImageName"].ToString(),
                                CategoryId = sdr["CategoryId"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public void InsertReviewDetails(string CatID, string SubcatID, int productRate, string reviewtitle, string reviewDes, string filename, string email )
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string sql = "dbo.InsertReviewDetails";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Connection = conn;
                    try
                    {
                        cmd.Parameters.Add("@pCatID", System.Data.SqlDbType.NVarChar).Value= CatID;
                        //cmd.Parameters["@pGuid"].Value = strguid;

                        cmd.Parameters.Add("@pSubCatID", System.Data.SqlDbType.NVarChar).Value= SubcatID;
                        //cmd.Parameters["@pEmail"].Value = umEmail;

                        cmd.Parameters.Add("@pProductRate", System.Data.SqlDbType.Int).Value = productRate;

                        cmd.Parameters.Add("@pReviewTitle", System.Data.SqlDbType.NVarChar).Value = reviewtitle;

                        cmd.Parameters.Add("@pReviewDes", System.Data.SqlDbType.NVarChar).Value = reviewDes;

                        cmd.Parameters.Add("@pFileName", System.Data.SqlDbType.NVarChar).Value = filename; 

                        cmd.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = email;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
    }
}