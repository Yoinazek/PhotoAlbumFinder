using Microsoft.Extensions.DependencyInjection;

namespace PhotoAlbum.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IPhotoAlbumAdmin, PhotoAlbumAdmin>()
                .AddSingleton<IAlbumFinderService, AlbumFinderService>()
                .AddSingleton<IApiClient, ApiClient>()
                .BuildServiceProvider();

            var admin = serviceProvider.GetService<IPhotoAlbumAdmin>();
            admin.Run();
        }
    }
}
