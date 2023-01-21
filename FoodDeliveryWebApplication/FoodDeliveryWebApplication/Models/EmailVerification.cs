using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace FoodDeliveryWebApplication.Models
{
    public class EmailVerification
    {
        

        public string SendEmail(string subjectText,string bodyText,string sentTo)
        {
          
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("admfcptest@gmail.com", "nyjmvsenvexksqiu");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = subjectText;
            msg.Body = bodyText;
            msg.To.Add(sentTo);
            msg.From = new MailAddress("admfcptest@gmail.com");
            msg.IsBodyHtml = true;
            try
            {
                smtp.Send(msg);
                return "Sent";
                
            }
            catch
            {
                throw;
            }

        }
    }
}