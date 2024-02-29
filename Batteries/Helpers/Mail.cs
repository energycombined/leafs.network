using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Batteries.Helpers
{
    public class Mail
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static bool SendMail(string strTo, string from, string subject, string strBody)
        {
            var myMail = new MailMessage();
            var sc = new SmtpClient();
            myMail.From = new MailAddress(from, from);
            myMail.To.Add(new MailAddress(strTo, strTo));
            myMail.Subject = subject;
            myMail.Priority = MailPriority.Normal;
            myMail.IsBodyHtml = true;
            myMail.Body = strBody;
            sc.Host = ConfigurationManager.AppSettings["mailServer"];
            sc.Port = Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"]);
            sc.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailUser"],
                ConfigurationManager.AppSettings["mailPass"]);
            sc.EnableSsl = true;
            try
            {
                sc.Send(myMail);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        //ne se koristi
        public static bool IsValidEmail(string email)
        {
            bool result = false;

            MailAddress mailAddress;
            try
            {
                mailAddress = new MailAddress(email);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}