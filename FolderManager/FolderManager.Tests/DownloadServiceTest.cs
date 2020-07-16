using FolderManager.Services;
using FolderManager.Services.Handlers;
using NUnit.Framework;
using Moq;
using FolderManager.Services.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Moq.Protected;
using System.Threading;

namespace FolderManager.Tests
{
    public class DownloadServiceTest
    {
        DownloadService service;

        Mock<IHttpHandler> httpFake;

        [SetUp]
        public void Setup()
        {
            httpFake = new Mock<IHttpHandler>();
            
        }

        [Test]
        public async Task DownloadFileNamesTest()
        {
            var json = "{ \"Files\":[\"test.pdf\",\"test.txt\"],\"ErrorMessage\":null}";
         var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                new StringContent(json),
            };

            httpFake
                .Setup(
                   x => x.GetAsync((It.IsAny<string>())) )

                .Returns(Task.FromResult<HttpResponseMessage>(response));
            
            service = new DownloadService(httpFake.Object);
            var fileNames = await service.DownloadFileNames();
            
            Assert.IsTrue(fileNames.Contains("test.pdf"));
            Assert.IsTrue(fileNames.Contains("test.pdf"));
        }

        [Test]
        public async Task DownloadTest_OK()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                    new StringContent("test"),
            };

            httpFake
                .Setup(
                   x => x.GetAsync((It.IsAny<string>())))

                .Returns(Task.FromResult<HttpResponseMessage>(response));

            service = new DownloadService(httpFake.Object);
            var fileNames = await service.Download("test.txt");

            Assert.IsTrue(fileNames);
        }

        [Test]
        public async Task DownloadTest_NOK()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content =
                    new StringContent("test"),
            };

            httpFake
                .Setup(
                   x => x.GetAsync((It.IsAny<string>())))

                .Returns(Task.FromResult<HttpResponseMessage>(response));

            service = new DownloadService(httpFake.Object);
            var fileNames = await service.Download("test.txt");

            Assert.IsTrue(!fileNames);
        }

        [Test]
        public async Task UploadTest_OK()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                    new StringContent("test"),
            };

            httpFake
                .Setup(
                   x => x.PostAsync(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()))

                .Returns(Task.FromResult<HttpResponseMessage>(response));

            service = new DownloadService(httpFake.Object);
            var isOK = await service.Upload("test.txt");

            Assert.IsTrue(isOK);
        }

        [Test]
        public async Task UploadTest_NOK()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content =
                    new StringContent("test"),
            };

            httpFake
                .Setup(
                   x => x.PostAsync(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>()))

                .Returns(Task.FromResult<HttpResponseMessage>(response));

            service = new DownloadService(httpFake.Object);
            var isOK = await service.Upload("test.txt");

            Assert.IsTrue(!isOK);
        }

    }
}