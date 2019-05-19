using MyReview.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
        public ActionResult AddReview(FormCollection f, HttpPostedFileBase file)
        {
            HttpStatusCode hp = InsertReviewDetails(f["hdCategoryId"].ToString(), f["hdSubCategoryId"].ToString(), Convert.ToInt16(f["hdStarRating"]), f["reviewtitle"].ToString(), f["comment"].ToString()
                , Convert.ToBoolean(f["optradio"]), file);

            if (hp.ToString() == "Created")
            {
                //success page
            }
            //else error page
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

        public HttpStatusCode InsertReviewDetails(string catID, string subcatID, int productRate, string reviewtitle, string reviewDes, bool isLikeProduct, HttpPostedFileBase file)
        {
            if (Session["LoggedInUser"] != null)
            {
                UserModel um = (UserModel)Session["LoggedInUser"];
                string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                string sql = "dbo.InsertReviewDetails";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        try
                        {
                            string fileName = string.Empty;
                            cmd.Parameters.Add("@pCatID", System.Data.SqlDbType.NVarChar).Value = catID;
                            cmd.Parameters.Add("@pSubCatID", System.Data.SqlDbType.NVarChar).Value = subcatID;
                            cmd.Parameters.Add("@pUserID", System.Data.SqlDbType.NVarChar).Value = um.UserId;
                            cmd.Parameters.Add("@pProductRate", System.Data.SqlDbType.Int).Value = productRate;
                            cmd.Parameters.Add("@pReviewTitle", System.Data.SqlDbType.NVarChar).Value = reviewtitle;
                            cmd.Parameters.Add("@pReviewDes", System.Data.SqlDbType.NVarChar).Value = reviewDes;
                            cmd.Parameters.Add("@pIsLikeProduct", System.Data.SqlDbType.Bit).Value = isLikeProduct;
                            if (file != null && file.ContentLength > 0)
                            {
                                Guid strGuid = Guid.NewGuid();
                                fileName = strGuid.ToString();
                                string imgPath = Path.Combine(Server.MapPath("~/ReviewUploads/" + fileName + Path.GetExtension(file.FileName)));
                                file.SaveAs(imgPath);
                                cmd.Parameters.Add("@pFileName", System.Data.SqlDbType.NVarChar).Value = fileName + Path.GetExtension(file.FileName);
                            }
                            
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return HttpStatusCode.BadRequest;
                        }
                    }
                }
            }
            else
            {
                return HttpStatusCode.Unauthorized;
            }
            return HttpStatusCode.Created;
        }
    }
}