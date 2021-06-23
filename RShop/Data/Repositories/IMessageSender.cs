using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RShop.Data.Repositories
{
  public interface IMessageSender
    {
        public Task SendEmailAsync(string toemail, string subject, string message, bool ismassageHtml = false);
    }
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toemail, string subject, string message, bool ismassageHtml = false)
        {
            using (var client=new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = "",
                    Password = ""
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toemail) },
                    From = new MailAddress(""),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = ismassageHtml
                };
                client.Send(emailMessage);
            }
            return Task.CompletedTask;
        }
    }
}
