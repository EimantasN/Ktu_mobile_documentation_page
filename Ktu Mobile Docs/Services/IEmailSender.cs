using System.Threading.Tasks;

namespace Ktu_Mobile_Docs.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}