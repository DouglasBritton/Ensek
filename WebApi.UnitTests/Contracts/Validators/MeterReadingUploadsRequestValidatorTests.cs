using Microsoft.AspNetCore.Http;
using Moq;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Validators;

namespace WebApi.UnitTests.Contracts.Validators
{
    [TestClass]
    public class MeterReadingUploadsRequestValidatorTests
    {
        private MeterReadingUploadsRequestValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new MeterReadingUploadsRequestValidator();
        }

        [TestMethod]
        [DataRow("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        [DataRow("text/csv")]
        public void MeterReadingUploadsRequestValidator_AcceptsCorrectFileTypes(string fileType)
        {
            // Arrange
            var file = new Mock<IFormFile>();
            file.Setup(x => x.ContentType).Returns(fileType);

            var entity = new MeterReadingUploadsRequest
            {
                File = file.Object
            };

            // Act
            var result = _validator.Validate(entity);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        [DataRow("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [DataRow("application/vnd.openxmlformats-officedocument.presentationml.presentation")]
        public void MeterReadingUploadsRequestValidator_RejectsInCorrectFileTypes(string fileType)
        {
            // Arrange
            var file = new Mock<IFormFile>();
            file.Setup(x => x.ContentType).Returns(fileType);

            var entity = new MeterReadingUploadsRequest
            {
                File = file.Object
            };

            // Act
            var result = _validator.Validate(entity);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The file type provided is not supported.", result.Errors[0].ErrorMessage);
        }
    }
}