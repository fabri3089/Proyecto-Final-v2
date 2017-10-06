using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class Mailing
    {
        public string Execute()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("cristian.pique33@gmail.com");
                mail.To.Add("cristian.pique@hotmail.com");
                mail.Subject = "Test Mail";
                mail.IsBodyHtml = true;
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("cristian.pique33@gmail.com", "joy34578644");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExecuteHTMLBody(string path)
        {
            try
            {
                using (StreamReader reader = File.OpenText(path)) 
                {                                                        
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("cristian.pique33@gmail.com");
                    mail.To.Add("cristian.pique@hotmail.com");
                    mail.Subject = "Test Mail";
                    mail.IsBodyHtml = true;
                    mail.Body = reader.ReadToEnd();
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("cristian.pique33@gmail.com", "joy34578644");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
