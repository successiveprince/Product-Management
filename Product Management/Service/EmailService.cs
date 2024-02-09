using System.Net.Mail;
using System.Net;

namespace Product_Management.Service
{
    public class EmailService
    {
        public Task SendEmail(string email, string subject, string body)
        {
            try
            {
                string fromMail = "prince.kumar1@successive.tech";
                string fromPassword = "upsh vtlt jocl ikyf";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(new MailAddress(email));
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };
                smtpClient.Send(message);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
