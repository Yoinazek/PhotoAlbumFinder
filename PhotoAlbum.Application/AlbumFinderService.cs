using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbum.Application
{
    public interface IAlbumFinderService
    {
        Task<List<PhotoModel>> FindAlbums(string albumId);
    }

    public class AlbumFinderService : IAlbumFinderService
    {
        private readonly IApiClient apiClient;

        public AlbumFinderService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<List<PhotoModel>> FindAlbums(string albumId)
        {
            string uri = ConfigurationManager.AppSettings["ApiUrl"] + $"?albumId={albumId}";
            List<PhotoModel> albums = new List<PhotoModel>();

            try
            {
                HttpResponseMessage response = await this.apiClient.GetStringAsync(uri);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                albums = JsonConvert.DeserializeObject<List<PhotoModel>>(result);
            }
            catch
            {
                Console.WriteLine($"There was an error getting album data. Please try again later.");
            }

            return albums;
        }
    }
}
