using DataAccess.Entities;
using FakeItEasy;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Contracts.Requests;
using WebApi.MeterReadings;
using WebApi.MeterReadings.Endpoints;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.UnitTests.MeterReadings.Endpoints
{
    [TestClass]
    public class MeterReadingUploadsEndpointTests
    {
        [TestMethod]
        public async Task MeterReadingUploadsEndpoint_ReturnsCorrectResponse()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var req = new MeterReadingUploadsRequest
            {
                File = file.Object
            };

            var data = new FileProcessedModel 
            {
                TotalProcessedEntries = 15,
                ValidEntries =
                [
                    new MeterReading
                    {
                        AccountId = 1,
                        ReadingDateTime = new DateTime(2024, 09, 16, 0, 0, 0, DateTimeKind.Utc),
                        ReadValue = "01230"
                    },
                    new MeterReading
                    {
                        AccountId = 1,
                        ReadingDateTime = new DateTime(2024, 10, 16, 0, 0, 0, DateTimeKind.Utc),
                        ReadValue = "01240"
                    },
                    new MeterReading
                    {
                        AccountId = 1,
                        ReadingDateTime = new DateTime(2024, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                        ReadValue = "01250"
                    },
                    new MeterReading
                    {
                        AccountId = 1,
                        ReadingDateTime = new DateTime(2024, 12, 16, 0, 0, 0, DateTimeKind.Utc),
                        ReadValue = "01260"
                    }
                ]
            };
            var fakeMeterReadingFileUploadsProcess = A.Fake<IMeterReadingFileUploadsProcess>();
            A.CallTo(() => fakeMeterReadingFileUploadsProcess.ProcessAsync(file.Object)).Returns(Task.FromResult(data));

            var fakeMeterReadingService = A.Fake<IMeterReadingService>();
            A.CallTo(() => fakeMeterReadingService.ImportMultipleAsync(data.ValidEntries)).Returns(Task.FromResult(3));

            var endpoint = Factory.Create<MeterReadingUploadsEndpoint>(fakeMeterReadingService,
                fakeMeterReadingFileUploadsProcess);

            // Act
            var rsp = await endpoint.ExecuteAsync(req, default);

            // Assert
            Assert.AreEqual(StatusCodes.Status200OK, rsp.StatusCode);
            Assert.AreEqual(3, rsp?.Value?.ReadingsAddedSuccessfully);
            Assert.AreEqual(12, rsp?.Value?.ReadingsAddedFailed);
        }
    }
}