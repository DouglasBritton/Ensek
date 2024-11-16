using Microsoft.AspNetCore.Http;
using Moq;
using WebApi.Contracts.Requests;
using WebApi.MeterReadings.Endpoints;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.UnitTests.MeterReadings.Endpoints
{
    [TestClass]
    public class MeterReadingUploadsEndpointTests
    {
        private MeterReadingUploadsEndpoint _endpoint;
        private Mock<IMeterReadingService> _mockMeterReadingService;
        private Mock<IMeterReadingFileUploadsProcess> _fileUploadProcess;

        [TestInitialize]
        public void Setup()
        {
            //TODO
            //_endpoint = MeterReadingUploadsEndpoint();
        }


        [TestMethod]
        public async Task MeterReadingUploadsEndpoint_()
        {
            // Arrange
            var file = new Mock<IFormFile>();

            var req = new MeterReadingUploadsRequest
            {
                File = file.Object
            };

            // Act
            await _endpoint.ExecuteAsync(req, default);
            var rsp = _endpoint.Response;


            // Assert
            //Assert.AreEqual(expectedValidation, result.IsValid);
        }
    }
}