using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphQLDotNet.Services
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient client, string requestUri)
        {
            var result = await client.GetStreamAsync(requestUri);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<T>(result, options);
        }
    }
}
