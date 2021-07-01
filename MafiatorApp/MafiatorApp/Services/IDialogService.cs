using System.Threading.Tasks;

namespace MafiatorApp.Services
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmedAsync();
        Task<string> ShowPickImageAsync();
    }
}
