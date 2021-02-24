//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Newtonsoft.Json;
//using PhotoAlbum.Application;

//namespace Tests
//{
//    [TestClass]
//    public class PhotoAlbumTests
//    {
//        [TestMethod]
//        public void HttpClientThrowsError()
//        {
//            Mock<IHttpClientFactory> client = new Mock<IHttpClientFactory>();

//            List<PhotoModel> albums = new List<PhotoModel>();
//            albums.Add(new PhotoModel());

//            var albumsString = JsonConvert.SerializeObject(albums);

//            Mock<HttpClient> hp = new Mock<HttpClient>();
//            hp.Setup(x => x.GetStringAsync(It.IsAny<string>())).Returns(Task.FromResult(albumsString));
//            client.Setup(x => x.Build()).Returns(hp.Object);
//            var sut = new Program(client.Object);
//            sut.Main();
//        }
//    }
//}
