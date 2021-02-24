using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum
{
    public class AlbumFinderResponse
    {
        public AlbumFinderResponse()
        {
            Albums = new List<PhotoModel>();
        }

        public List<PhotoModel> Albums { get; set; }
        public string Message { get; set; }
        public ResponseCode ResponseCode { get; set; }
    }

    public enum ResponseCode
    {
        Success,
        Failure
    }
}
