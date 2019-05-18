using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyReview.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using MyReview.Common;


namespace MyReview.Controllers
{
    public class NewRegController : Controller
    {
        // GET: NewReg
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserModel um, HttpPostedFileBase file)
        {
            string fileName = string.Empty;
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!CheckingEmailDup(um.Email))
            {
                if (!ModelState.IsValid)
                {
                    CheckingEmailDup(um.Email);
                    SqlConnection sqlconn = new SqlConnection(connStr);
                    string sqlquery = "insert into dbo.UserMaster (UserName,PhoneNumber,Email,Password,Photo,UserType,CreateDate) values (@UName,@PNumber,@Email,@Pwd,@Photo,@Utype,@CreateDate)";
                    SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                    sqlcomm.Parameters.AddWithValue("@UName", um.UserName);
                    sqlcomm.Parameters.AddWithValue("@PNumber", um.PhoneNumber);
                    sqlcomm.Parameters.AddWithValue("@Email", um.Email);
                    sqlcomm.Parameters.AddWithValue("@Pwd", um.Password);
                    sqlcomm.Parameters.AddWithValue("@Utype", 0);

                    if (file != null && file.ContentLength > 0)
                    {
                        Guid strGuid = Guid.NewGuid();
                        fileName = strGuid.ToString();
                        string imgPath = Path.Combine(Server.MapPath("~/UserProfilePhoto/" + fileName + Path.GetExtension(file.FileName)));
                        file.SaveAs(imgPath);
                        sqlcomm.Parameters.AddWithValue("@Photo", fileName);
                    }
                    //else
                    //{
                    //    sqlcomm.Parameters.AddWithValue("@Photo", fileName);
                    //}
                    sqlcomm.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    sqlconn.Open();

                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();
                    ViewData["Message"] = "User Record " + um.UserName + " is saved successfully";

                    Common.Common obj = new Common.Common();
                    string strguid = obj.SendEmailValidation(um.Email, um.UserName);
                    InsertUserEmailDetails(strguid, um.Email);
                }
            }
            else
            {
                ViewData["Message"] = "Already this email have been registered, please try another email id";
            }
            return View();
        }
        public bool CheckingEmailDup(string umEmail)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string sql = "Select * from dbo.UserMaster where Email = '" + umEmail + "'";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Connection = conn;
                    try
                    {
                        string JSONString = string.Empty;
                        cmd.Connection.Open();
                        IDataReader reader = cmd.ExecuteReader();
                        DataTable dtSafetyProcess = new DataTable();
                        dtSafetyProcess.Load(reader);
                        if (dtSafetyProcess.Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void InsertUserEmailDetails(string strguid, string umEmail)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string sql = "dbo.InsertEmailVerficationDetails";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    cmd.Connection = conn;
                    try
                    {
                        cmd.Parameters.Add("@pGuid", System.Data.SqlDbType.VarChar);
                        cmd.Parameters["@pGuid"].Value = strguid;

                        cmd.Parameters.Add("@pEmail", System.Data.SqlDbType.Int);
                        cmd.Parameters["@pEmail"].Value = umEmail;

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
        public ActionResult EmailVerfication(string refId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string sql = "dbo.UpdateEmailCheckingStatus";
            if (GetEmailVerificationCount(refId) > 0)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        try
                        {
                            cmd.Parameters.Add("@pGuid", System.Data.SqlDbType.VarChar);
                            cmd.Parameters["@pGuid"].Value = refId;

                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Connection = conn;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "Sorry your email verification can't be processed now";
                            return View();
                        }
                    }
                }
            }
            else
            {
                ViewBag.Message = "No email verification for this link or already verification may be completed";
                return View();
            }
            ViewBag.Message = "Email Verification completed successfully";
            return View();
        }

        public int GetEmailVerificationCount(string pRefId)
        {
            string strQuery = "select count(*) from [dbo].[UserMaster] where [IsEmailValidated] = 0 and UserId = (select UserId from [dbo].[EmailValidationDetails] where [RefId] = '" + pRefId + "' )";

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(strQuery))
                {
                    cmd.Connection = conn;
                    try
                    {
                        cmd.Connection.Open();
                        IDataReader reader = cmd.ExecuteReader();
                        DataTable dUserCount = new DataTable();
                        dUserCount.Load(reader);
                        if (dUserCount.Rows.Count > 0)
                        {
                            return dUserCount.Rows.Count;
                        }
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
                }
            }
        }
    }
}