using System.Collections.Generic;

namespace GraphQLDotNet.Mobile.Services.Analytics
{
    public static class LocationAnalytics
    {
        public static void AddedALocation()
            => Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Added a location");

        public static void HasNLocations(int numberOfLocations)
            => Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Number of locations",
                new Dictionary<string, string> {
                    { "Number", numberOfLocations.ToString() }
                });
    }
}
