using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;

namespace Rebates1.Models
{

    public class EmailHelper
    {
        public bool SendEmailTwoFactorCode(string userEmail, string code)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("care@yogihosting.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Two Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("care@yogihosting.com", "yourpassword");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Objections@joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "You recently created the account below. Before you can log on you must first confirm your account information. <br/><br/>" +
                "UserName: " + userEmail + "<br/><br/>" +
                "Activate your account now by clicking this link: <a href=" + confirmationLink + "> here </a><br/><br/>" +
                 "<br/><br/>" +
                "Regards," + "<br/>" + "COJ - City of Johannesburg Metropolitan Municipality";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Objections@joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        private bool RedirectToPage(string v, object value)
		{
			throw new NotImplementedException();
		}

		public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Objections@joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Change Password";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Hi " + userEmail + "<br/><br/> Forgot password? <br/><br/> We received a request to reset the password for your account. <br/><br/>" +
                "To reset your password, click this link: <a href=" + link + "> here </a><br/><br/>" +
             "Regards," + "<br/>" + "COJ - City of Johannesburg Metropolitan Municipality";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Objections@joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return true;
                //return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailNotificationReject(string userEmail)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Rebates@Joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Application Auto Rejected";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Dear: " + userEmail + "<br/><br/>" + "Thank you for taking time to apply for the rebate. <br/><br/> " +
                "We regret to inform you that your rebate application was unsuccessful." +
                "<br/><br/>" + "Possible reasons for automatic rejection:" +
                "<br/>" +
                "A) Applicant failing to meet the minimum age requirement " + "<br/>" +
                "B) Applicant failing to meet the occupancy requirement for the rebate" +
                "Regards," + "<br/>" + "COJ - City of Johannesburg Metropolitan Municipality";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Rebates@Joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailNotificationSuccessful(string userEmail)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Rebates@Joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Thank you for applying";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Dear: " + userEmail + "<br/><br/>" + "Thank you for taking time to apply for the rebate. <br/><br/> " +
                "We would like to let you know that we have received your application. Our team will evaluate your application and will let you know the outcomes." +
                "<br/><br/>" +
                "Regards," + "<br/>" + "COJ - City of Johannesburg Metropolitan Municipality";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Rebates@Joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailNotificationRebateApprove(string userEmail, string sRebateNo) 
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Rebates@Joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Application Reference Number: " + sRebateNo.ToString();
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Good day Valued Customer <br/><br/>" + "Thank you for utilizing the online rebate process. <br/><br/> " +
                "Your application for the rebate has been processed and the details are below:" +
                "<br/><br/>" +
                "Rebate Reference: "  + sRebateNo.ToString() + "<br/>" +
                "Application Outcome: Approved <br/>" +
                "Comment: Pensioners` rebate application was approved retrospectively from the application date for the Valuation period 1 July 2023 until 30 June 2027. The approved rebate and the credited amount will appear on your invoice. <br/><br/>" +
                "If you have any question regarding the above, use the below contact details.<br/>" +
                "Bonginkosid@joburg.org.za <br/><br/>" +
                "Note: This is an unattended email, and no replies will be made by this email address <br/>" +
                "Regards,";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Rebates@Joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailNotificationRebateReject(string userEmail, string sRebateNo)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("Rebates@Joburg.org.za");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Application Reference Number: " + sRebateNo.ToString();
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Good day Valued Customer <br/><br/>" + "Thank you for utilizing the online rebate process. <br/><br/> " +
                "Your application for the rebate has been processed and the details are below:" +
                "<br/><br/>" +
                "Rebate Reference: " + sRebateNo.ToString() + "<br/>" +
                "Application Outcome: Rejected <br/>" +
                "Comment: : Pensioners` rebate application for the Valuation period 1 July 2023 until 30 June 2027 was rejected due to some of the below listed reasons: <br/><br/>" +
                "Applicant is not the registered owner of the property.<br/>" +
                "Incomplete supporting documentation. ( e.g. ID copies that are not certified or without a date stamp).<br/>" +
                "Income of the registered owner, in the case of pensioners between 60-70 years of age, exceeds the threshold income amounting to R 20 404.<br/>" +
                "Bonginkosid@joburg.org.za <br/><br/>" +
                "Note: This is an unattended email, and no replies will be made by this email address <br/>" +
                "Regards,";
            SmtpClient client = new SmtpClient();
            //client.Host = "168.89.180.52";
            client.Host = "cojmail.joburg.org.za";
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("Rebates@Joburg.org.za", "");
            client.UseDefaultCredentials = false;
            client.Port = 25;

            try
            {
                client.Send(mailMessage);
                return RedirectToPage("/Account/RegisterConfirmation", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}
