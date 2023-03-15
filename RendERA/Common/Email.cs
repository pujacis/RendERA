using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RendERA.Common
{
   
    public class Email
    {
        public bool Sendmailwithsubject(string emailid, string sub, string msg)
        {
            try
            {
                var client = new SmtpClient("smtp.googlemail.com", 587)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Timeout = 20000,
                    Credentials = new NetworkCredential("deepak.m@cisinlabs.com", "mQhtWthnj3")  // gmailid is someguy@gmail.com
                };

                MailMessage message = new MailMessage("deepak.m@cisinlabs.com", emailid, sub, msg);

                client.Send(message);
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

    }
}