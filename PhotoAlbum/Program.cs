using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                .AddSingleton<IPhotoAlbumAdmin, PhotoAlbumAdmin>()
                .AddSingleton<IAlbumFinderService, AlbumFinderService>()
                .AddSingleton<IApiClient, ApiClient>()
                .BuildServiceProvider();

                var admin = serviceProvider.GetService<IPhotoAlbumAdmin>();
                await admin.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
