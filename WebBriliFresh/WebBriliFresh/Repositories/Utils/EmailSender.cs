using MailKit.Net.Smtp;
using MimeKit;

namespace WebBriliFresh.Utils
{
    public class EmailSender : IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("brilifreshweb@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = htmlMessage};
            
            //send email
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("brilifreshweb@gmail.com", "hedgdnfkbzirqbsa");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }


            return Task.CompletedTask;
        }
    }
}
