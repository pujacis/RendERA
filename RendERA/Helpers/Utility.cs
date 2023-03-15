using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.Helpers
{
    public class Utility
    {
        public static string GenerateRandomCode()
        {
            try {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(System.Linq.Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            catch (Exception ex) { throw ex; }
        }

        public static void SendEmail(  string Emailid, string Subject, string HtmlBody)
        {
            try
            {
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
                //you need also allow less secure app by gmail setting here https://myaccount.google.com/u/1/lesssecureapps
                m = new System.Net.Mail.MailMessage(
                new System.Net.Mail.MailAddress("renderafarm@gmail.com", "RendERA"),
                new System.Net.Mail.MailAddress(Emailid));
                //  }
                m.Subject =  Subject;
                m.Body = HtmlBody ;
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential("renderafarm@gmail.com", "Am000000007");
                smtp.EnableSsl = true;
                //smtp.EnableSsl = false;
                smtp.Send(m);
               
            }
            catch (Exception ex)
            {
                //The SMTP server requires a secure connection or the client was not authenticated.
                //The server response was: 5.7.0 Authentication Required. Learn more at
                throw ex;
            }
        }



    }
}
