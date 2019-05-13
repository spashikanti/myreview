using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace MyReview.Common
{
    public class Common
    {
        public string SendEmailValidation(string umEmail, string umName)
        {
            
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string FromEmail = ConfigurationManager.AppSettings["EmailValidateSendEmail"].ToString();
            string DisplayName = ConfigurationManager.AppSettings["DisplayName"].ToString();
            string Password = ConfigurationManager.AppSettings["fromPassword"].ToString();
            string csmtp = ConfigurationManager.AppSettings["smtp"].ToString();
            string portno = ConfigurationManager.AppSettings["portno"].ToString();
            string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();
            Guid obj = Guid.NewGuid();

            StringBuilder sb = new StringBuilder();
            sb.Append("Hi ");
            sb.Append(umName);
            sb.Append("\n");
            sb.Append("This email is to verify your email address and activate your acount in MyReview portal\n");
            sb.Append("Please click <a href='");
            sb.Append(siteURL);
            sb.Append(obj.ToString());
            sb.Append("'>here</a>");
            sb.Append("\n\n\n\n");
            sb.Append("Thank you\n");
            sb.Append("MyReview");

            var fromAddress = new MailAddress(FromEmail, DisplayName);
            var toAddress = new MailAddress(umEmail, umName);
            string fromPassword = Password;
            string subject = "Email Validation";
            string body = sb.ToString();

            var smtp = new SmtpClient
            {
                Host = csmtp,
                Port = Convert.ToInt16(portno),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                //smtp.Send(message);
            }
            return obj.ToString();
        }
    }
}