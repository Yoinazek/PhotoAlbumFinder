using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    public interface IAlbumFinderService
    {
        Task<AlbumFinderResponse> FindAlbums(string albumId);
    }

    public class AlbumFinderService : IAlbumFinderService
    {
        private readonly IApiClient apiClient;

        public string RequestUri { get; set; }

        public AlbumFinderService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<AlbumFinderResponse> FindAlbums(string albumId)
        {
            if (!string.IsNullOrWhiteSpace(albumId) && !int.TryParse(albumId, out int _))
            {
                return new AlbumFinderResponse()
                {
                    Message = ResponseMessages.InvalidInput,
                    ResponseCode = ResponseCode.Failure
                };
            }

            RequestUri = AlbumApiLocation.ApiUrl;
            if (!string.IsNullOrWhiteSpace(albumId))
                RequestUri += $"?albumId={albumId}";

            List<PhotoModel> albums = new List<PhotoModel>();

            try
            {
                HttpResponseMessage response = await this.apiClient.GetAsync(RequestUri);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                albums = JsonConvert.DeserializeObject<List<PhotoModel>>(result);
            }
            catch
            {
                //You would log an exception here before returning
                return new AlbumFinderResponse()
                {
                    Message = ResponseMessages.ApiError,
                    ResponseCode = ResponseCode.Failure
                };
            }

            return new AlbumFinderResponse()
            {
                Albums = albums,
                ResponseCode = ResponseCode.Success
            };
        }
    }
}
