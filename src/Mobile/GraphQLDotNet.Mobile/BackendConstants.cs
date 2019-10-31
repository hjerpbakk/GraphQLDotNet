using Xamarin.Forms;

namespace GraphQLDotNet.Mobile
{
    public static class BackendConstants
    {
#if DEBUG
        public static string GraphQLApiUrl { get; } = Device.RuntimePlatform is Device.Android ? "http://10.0.2.2:5000/graphql" : "http://localhost:5000/graphql";
#else
#error Missing GraphQL Api Url
        public static string GraphQLApiUrl { get; } = "";
#endif
    }
}
