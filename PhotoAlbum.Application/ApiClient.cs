using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbum.Application
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetStringAsync(string uri);
    }

    public class ApiClient : IApiClient
    {
        readonly HttpClient client = new HttpClient();

        public Task<HttpResponseMessage> GetStringAsync(string uri)
        {
            return client.GetAsync(uri);
        }
    }
}
