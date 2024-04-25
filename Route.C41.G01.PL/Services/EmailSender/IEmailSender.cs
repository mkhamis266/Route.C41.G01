using System.Threading.Tasks;

namespace Route.C41.G01.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		Task SendAsync(string from, string recipiens,string subject,string body);	
	}
}
