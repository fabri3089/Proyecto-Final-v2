using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Services
{
    public class SendGridMailing
    {
        public void Execute(string templatePath, string email, string pass)
        {
            HelloEmail(templatePath, email, pass).Wait();
        }

        public void Execute(string template, List<String> emails)
        {
            CampaignEmail(template, emails).Wait();
        }

        /// <summary>
        /// Email de bienvenida al registrarse un usuario en el gimnasio
        /// </summary>
        /// <param name="templatePath">Ruta de template a utilizar</param>
        /// <param name="email">Email del usuario a inyectar en template</param>
        /// <param name="pass">Password generada por el admin a inyectar en template</param>
        /// <returns></returns>
        private static async Task HelloEmail(string templatePath, string email, string pass)
        {
            String apiKey = Environment.GetEnvironmentVariable("ENVIRONMENT_VARIABLE_SENDGRID_KEY");
            dynamic sg = new SendGrid.SendGridAPIClient(apiKey, "https://api.sendgrid.com");

            Email from = new Email("cristianpique33@gmail.com");
            String subject = "Bienvenido a AmosGym";
            Email to = new Email(email);

            Content content;
            if (string.IsNullOrEmpty(templatePath))
            {
                content = new Content("text/plain", "Te damos la bienvenida a AMOS Gym. Visita nuestro sitio web http://amosgym.azurewebsites.net/ para enterarte de las últimas novedades y personalizar tu información");
            }
            else
            {
                var emailTemplate = File.ReadAllText(templatePath);
                emailTemplate = emailTemplate.Replace("USER_REPLACE_TEXT", email)
                                             .Replace("PASS_REPLACE_TEXT", pass);
                content = new Content("text/html", emailTemplate);
            }
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
            var statusCode = (response.StatusCode);
            var result = (response.Body.ReadAsStringAsync().Result);
            var headers = (response.Headers.ToString());
            var get = mail.Get();

        }

        /// <summary>
        /// Campaña generada. Comunicacion masiva de emails
        /// </summary>
        /// <param name="template">Template con datos inyectados a utilizar</param>
        /// <param name="emails">Emails de usuarios a comunicar</param>
        /// <returns></returns>
        private static async Task CampaignEmail(string template, List<String> emails)
        {
            const string SUBJECT = "Comunicación de Amos Gym";

            String apiKey = Environment.GetEnvironmentVariable("ENVIRONMENT_VARIABLE_SENDGRID_KEY");
            dynamic sg = new SendGrid.SendGridAPIClient(apiKey, "https://api.sendgrid.com");

            Content content = new Content("text/html", template);

            Email from = new Email("cristianpique33@gmail.com");
            Email to = new Email(emails[0]);

            Mail mail = new Mail(from, SUBJECT, to, content);

            if (emails.Count > 1)
            {
                for (int i = 1; i < emails.Count; i++)
                {
                    Email email = new Email(emails[i]);
                    mail.Personalization[0].AddTo(email);
                }
            }

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
            var statusCode = (response.StatusCode);
            var result = (response.Body.ReadAsStringAsync().Result);
            var headers = (response.Headers.ToString());
            var get = mail.Get();

        }
    }
}
