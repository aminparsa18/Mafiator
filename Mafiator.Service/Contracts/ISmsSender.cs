using System.Threading.Tasks;

namespace Mafiator.Service.Contracts
{
    public interface ISmsSender
    {
        string SendAuthSmsAsync(string code, string phoneNumber);
    }
}
