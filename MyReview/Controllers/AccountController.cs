using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MyReview.Models;
using System.IO;




namespace MyReview.Controllers
{
    public class AccountController : Controller
    {
        
        public ActionResult Validate(FormCollection f, string returnUrl)
        {
            string r = ViewBag.ReturnUrl;
            if (IsEmptyValidate(f["txtUserName"].ToString(), f["txtpwd"].ToString()))
            {
                string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                string strUser = "Select * from UserMaster where IsEmailValidated = 0 and Email  ='" + f["txtUserName"] + "' and Password = '" + f["txtpwd"] + "'";
                using (SqlConnection connection =
               new SqlConnection(connStr))
                {
                    //SqlCommand command = new SqlCommand(strUser, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(strUser, connection);
                    DataSet userDetails = new DataSet();
                    try
                    {
                        UserModel loggedInUser = null;
                        adapter.Fill(userDetails, "UserMaster");

                        if(userDetails.Tables["UserMaster"].Rows.Count > 0)
                        {
                            System.Web.Security.FormsAuthentication.SetAuthCookie(f["txtUserName"].ToString(), false);
                            loggedInUser = new UserModel();

                            foreach (DataRow pRow in userDetails.Tables["UserMaster"].Rows)
                            {
                                loggedInUser.UserId = Convert.ToInt16(pRow["UserId"]);
                                loggedInUser.UserName = Convert.ToString(pRow["UserName"]);
                                loggedInUser.UserType = Convert.ToInt16(pRow["UserType"]);
                                if (pRow["Photo"] != null && !string.IsNullOrEmpty(pRow["Photo"].ToString()))
                                {
                                    string imgPath = Path.Combine(Server.MapPath("~/UserProfilePhoto/" + pRow["Photo"].ToString()));
                                    loggedInUser.Photo = imgPath;
                                }

                                loggedInUser.PhoneNumber = Convert.ToString(pRow["PhoneNumber"]);
                                loggedInUser.IsEmailValidated = Convert.ToBoolean(pRow["IsEmailValidated"]);
                                loggedInUser.Email = Convert.ToString(pRow["Email"]);

                                Session["LoggedInUser"] = loggedInUser;

                                //if (Convert.ToBoolean(pRow["UserType"]))
                                //    return RedirectToAction("Index", "Administrator"); 
                                //else
                                //    return RedirectToAction("Index", "Review");
                            }
                            if (!string.IsNullOrEmpty(returnUrl))
                                return RedirectToLocal(returnUrl);
                            if (Convert.ToBoolean(loggedInUser.UserType))
                                return RedirectToAction("Index", "Administrator");
                            return RedirectToAction("Index", "Review");
                        }
                        else
                            ViewBag.msg = "Invalide Email / Password";
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return View("Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Review");
        }
        public ActionResult login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: Account
        public ActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewUser(UserModel um, HttpPostedFileBase photo)
        {
            string fileName = string.Empty;
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SqlConnection sqlconn = new SqlConnection(connStr);
            string sqlquery = "insert into dbo.UserMaster (UserName,PhoneNumber,Email,Password,Photo,UserType) values (@UName,@PNumber,@Email,@Pwd,@Photo,@Utype)";
            SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
            sqlcomm.Parameters.AddWithValue("@UName",um.UserName);
            sqlcomm.Parameters.AddWithValue("@PNumber",um.PhoneNumber);
            sqlcomm.Parameters.AddWithValue("@Email",um.Email);
            sqlcomm.Parameters.AddWithValue("@Pwd",um.Password);
            sqlcomm.Parameters.AddWithValue("@Utype",0);
            if(photo !=null && photo.ContentLength > 0)
            {
                Guid obj = Guid.NewGuid();
                fileName = obj.ToString();
                string imgPath = Path.Combine(Server.MapPath("~/UserProfilePhoto/" + fileName));
                photo.SaveAs(imgPath);
                sqlcomm.Parameters.AddWithValue("@Photo", fileName);

            }
            sqlcomm.Parameters.AddWithValue("@Photo", fileName);
            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();
            ViewData["Message"] = "User Record " + um.UserName + " is saved successfully";
            return View();
        }
        public bool IsEmptyValidate(string UserName,string Password)
        {
            if(UserName == "" && Password == "")
            {
                ViewBag.msg = "Enter Email and Password";
                return false;
            }
            else if(UserName == "" && Password != "")
            {
                ViewBag.msg = "Enter Email";
                return false;
            }
            else if (UserName != "" && Password == "")
            {
                ViewBag.msg = "Enter Password";
                return false;
            }

            return true;
        }

    }
}