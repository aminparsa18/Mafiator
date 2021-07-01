using System.Threading.Tasks;

namespace Mafiator.Service.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
