using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Wibci.CountryReverseGeocode;
using Wibci.CountryReverseGeocode.Models;
using Xamarin.Essentials;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public sealed class CountryLocator : ICountryLocator
    {
        const string DefaultCountry = "";

        private readonly CountryReverseGeocodeService reverseGeocodeService;
        private readonly Lazy<Dictionary<string, string>> iso3ToIso2Mapping;

        public CountryLocator(CountryReverseGeocodeService reverseGeocodeService)
        {
            this.reverseGeocodeService = reverseGeocodeService;
            iso3ToIso2Mapping = new Lazy<Dictionary<string, string>>(() =>
            CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.LCID))
                .GroupBy(ri => ri.ThreeLetterISORegionName)
                .ToDictionary(g => g.Key, g => g.First().TwoLetterISORegionName));
        }

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

                return GetCountryCodeFromLocation();

                string GetCountryCodeFromLocation()
                {
                    var locationInfo = reverseGeocodeService.FindCountry(new GeoLocation { Latitude = location.Latitude, Longitude = location.Longitude });
                    if (string.IsNullOrEmpty(locationInfo?.Id))
                    {
                        return DefaultCountry;
                    }

                    if (iso3ToIso2Mapping.Value.TryGetValue(locationInfo.Id, out string alpha2Code))
                    {
                        return alpha2Code;
                    }

                    return DefaultCountry;
                }
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
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return DefaultCountry;
            }
        }
    }
}
