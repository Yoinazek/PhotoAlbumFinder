using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PhotoAlbum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoAlbumTests
{
    [TestClass]
    public class AlbumFinderServiceTests
    {
        [TestMethod]
        public async Task AlbumFoundAsync()
        {
            var album = new List<PhotoModel>()
            {
                new PhotoModel()
                {
                    AlbumId = 1,
                    Id = 1,
                    Title = "test1"
                },
                new PhotoModel()
                {
                    AlbumId = 1,
                    Id = 2,
                    Title = "test2"
                }

            };
            var input = "1";
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(album))
            };
            Mock<IApiClient> apiClient = new Mock<IApiClient>();
            apiClient.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(response));

            var sut = GetServiceUnderTest(apiClient);
            var result = await sut.FindAlbums(input);

            Assert.AreEqual(ResponseCode.Success, result.ResponseCode);
            Assert.AreEqual(1, result.Albums.First(x => x.Id == 1).AlbumId);
            Assert.AreEqual("test1", result.Albums.First(x => x.Id == 1).Title);
            Assert.AreEqual(1, result.Albums.First(x => x.Id == 2).AlbumId);
            Assert.AreEqual("test2", result.Albums.First(x => x.Id == 2).Title);
            Assert.AreEqual(album.Count, result.Albums.Count);
            Assert.IsTrue(sut.RequestUri.EndsWith(input));
        }

        [TestMethod]
        public async Task ApiClientThrows()
        {
            var album = new List<PhotoModel>()
            {
                new PhotoModel()
                {
                    AlbumId = 3,
                    Id = 3,
                    Title = "test"
                }
            };
            var input = "3";
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(album))
            };
            Mock<IApiClient> apiClient = new Mock<IApiClient>();
            apiClient.Setup(x => x.GetAsync(It.IsAny<string>())).Throws(new Exception());

            var sut = GetServiceUnderTest(apiClient);
            var result = await sut.FindAlbums(input);

            Assert.AreEqual(ResponseCode.Failure, result.ResponseCode);
            Assert.AreEqual(ResponseMessages.ApiError, result.Message);
            Assert.AreEqual(0, result.Albums.Count);
            Assert.IsTrue(sut.RequestUri.EndsWith(input));
        }

        [TestMethod]
        public async Task NotNumbersFail()
        {
            Mock<IApiClient> apiClient = new Mock<IApiClient>();
            var sut = GetServiceUnderTest(apiClient);
            var result = await sut.FindAlbums("aWord");

            Assert.AreEqual(ResponseCode.Failure, result.ResponseCode);
            Assert.AreEqual(ResponseMessages.InvalidInput, result.Message);
            Assert.AreEqual(0, result.Albums.Count);
            Assert.IsNull(sut.RequestUri);
        }

        [TestMethod]
        public async Task NotFoundReturnsSuccess()
        {
            var input = "3";
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new List<PhotoModel>()))
            };
            Mock<IApiClient> apiClient = new Mock<IApiClient>();
            apiClient.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(response));
            var sut = GetServiceUnderTest(apiClient);
            var result = await sut.FindAlbums(input);

            Assert.AreEqual(ResponseCode.Success, result.ResponseCode);
            Assert.AreEqual(0, result.Albums.Count);
            Assert.IsTrue(sut.RequestUri.EndsWith(input));
        }

        [TestMethod]
        public async Task EmptyInputSearchesFullList()
        {
            var album = new List<PhotoModel>()
            {
                new PhotoModel()
                {
                    AlbumId = 3,
                    Id = 3,
                    Title = "test"
                }
            };
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(album))
            };
            Mock<IApiClient> apiClient = new Mock<IApiClient>();
            apiClient.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(response));
            var sut = GetServiceUnderTest(apiClient);
            var result = await sut.FindAlbums("");

            Assert.AreEqual(ResponseCode.Success, result.ResponseCode);
            Assert.AreEqual(album.Count, result.Albums.Count);
            Assert.AreEqual(AlbumApiLocation.ApiUrl, sut.RequestUri);
        }

        private AlbumFinderService GetServiceUnderTest(Mock<IApiClient> client)
        {
            return new AlbumFinderService(client.Object);
        }
    }
}
