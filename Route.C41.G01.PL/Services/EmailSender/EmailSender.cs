using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Route.C41.G01.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configs;

		public EmailSender(IConfiguration configs)
        {
			_configs = configs;
		}
        public async Task SendAsync(string from, string recipiens, string subject, string body)
		{
			
			var emailMessage = new MailMessage();
			emailMessage.From = new MailAddress(from);
			emailMessage.To.Add(recipiens);
			emailMessage.Subject = subject;
			emailMessage.Body = $"<html><body>{body}</body></html>";
			emailMessage.IsBodyHtml = true;

			var smtp = new SmtpClient(_configs["EmailSettings:SmtpClientServer"], int.Parse(_configs["EmailSettings:SmtpClientPort"]))
			{
				Credentials = new NetworkCredential(_configs["EmailSettings:SenderEmail"], _configs["EmailSettings:SenderPassword"]),
				EnableSsl = true
			};
			await smtp.SendMailAsync(emailMessage);
		}
	}
}
