using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MafiatorApp.Services
{
    public interface IGeoLocator
    {
        Task<Location> GetCurrentLocation();
    }
}
