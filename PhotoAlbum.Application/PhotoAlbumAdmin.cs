using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbum.Application
{
    public interface IPhotoAlbumAdmin
    {
        Task Run();
    }

    public class PhotoAlbumAdmin : IPhotoAlbumAdmin
    {
        private bool _exit;
        private readonly IAlbumFinderService albumFinderService;

        public PhotoAlbumAdmin(IAlbumFinderService albumFinderService)
        {
            this.albumFinderService = albumFinderService;
        }

        public async Task Run()
        {
            while (!_exit)
            {
                Console.WriteLine("Enter 'exit' to stop the program");
                Console.WriteLine("Enter album ID to filter by: ");
                var input = Console.ReadLine()?.Trim()?.ToLower();

                if (input == "exit")
                {
                    _exit = true;
                    continue;
                }
                else
                {
                    var albums = await this.albumFinderService.FindAlbums(input);
                    WriteAlbumData(input, albums);
                }
            }
        }

        private static void WriteAlbumData(string id, List<PhotoModel> albums)
        {
            if (albums.Count > 0)
            {
                Console.WriteLine($"Photo album {id}.");
                foreach (var album in albums)
                {
                    Console.WriteLine($"[{album.Id}] {album.Title}");
                }
            }
            else
            {
                Console.WriteLine($"No matching data was found for albumId {id}.");
            }
        }
    }
}
