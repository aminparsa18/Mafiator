using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MafiatorApp.Services.Impl
{
   public class GeoLocator:IGeoLocator
    {
        public async Task<Location> GetCurrentLocation()
        {
            var location=await Geolocation.GetLastKnownLocationAsync();
          if (location != null) return location;
          var request = new GeolocationRequest(GeolocationAccuracy.Medium); 
          location = await Geolocation.GetLocationAsync(request);
          return location;

        }
    }
}
