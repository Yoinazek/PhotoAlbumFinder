using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string uri);
    }

    public class ApiClient : IApiClient
    {
        readonly HttpClient client = new HttpClient();

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return client.GetAsync(uri);
        }
    }
}
