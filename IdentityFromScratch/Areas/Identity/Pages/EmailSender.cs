using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityFromScratch.Areas.Identity.Pages.Account
{
    public class EmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient("mail.file-tool.okyam.net");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("app@file-tool.okyam.net", "Link3721840!");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("app@file-tool.okyam.net");
            mailMessage.To.Add(email);
            mailMessage.Body = htmlMessage;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            await client.SendMailAsync(mailMessage);
        }
    }
}