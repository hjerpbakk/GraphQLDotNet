using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public class CountryLocator
    {
        const string DefaultCountry = "";

        public async Task<string> GetCurrentCountry()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Lowest);
                    location = await Geolocation.GetLocationAsync(request);
                    if (location == null)
                    {
                        return DefaultCountry;
                    }
                }

                return GetCountryCodeFromLocation(location);
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                return DefaultCountry;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                return DefaultCountry;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                return DefaultCountry;
            }
            catch (Exception)
            {
                // Unable to get location
                return DefaultCountry;
            }
        }

        // TODO: finn land fra coordinatene
        private string GetCountryCodeFromLocation(Location location)
        {
            return "NO";
        }
    }
}
