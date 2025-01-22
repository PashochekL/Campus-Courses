using MailKit.Net.Smtp;
using MimeKit;

namespace CampusCourses.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public async Task SendMessage(Message message)
        {
            var emailMessage = CreateMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateMessage(Message message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("", _emailConfiguration.From));

            foreach (var address in message.To)
            {
                emailMessage.To.Add(address);
            }

            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = message.Content
            };

            return emailMessage;
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, false);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    throw new Exception("Ошибка при попытке отправки сообщения", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
