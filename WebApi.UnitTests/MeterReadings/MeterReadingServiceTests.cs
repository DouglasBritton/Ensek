using DataAccess;
using DataAccess.Entities;
using FluentValidation;
using FluentValidation.Results;
using MockQueryable.Moq;
using Moq;
using WebApi.MeterReadings;

namespace WebApi.UnitTests.MeterReadings
{
    [TestClass]
    public class MeterReadingServiceTests
    {
        private MeterReadingService _service;
        private Mock<IValidator<MeterReading>> _mockMeterReadingValidator;
        private Mock<ApplicationDbContext> _mockApplicationDbContext;

        [TestInitialize]
        public void Setup()
        {
            var _mockMeterReadingValidator = new Mock<IValidator<MeterReading>>();
            var _mockApplicationDbContext = new Mock<ApplicationDbContext>();

            _service = new MeterReadingService(_mockMeterReadingValidator.Object, _mockApplicationDbContext.Object);
        }

        [TestMethod]
        public async void ImportMultipleAsync_OnlyAllowsValid()
        {
            // Arrange
            var imports = new List<MeterReading>
            {
                new MeterReading
                {
                    AccountId = 1,
                    ReadingDateTime = new DateTime(2024, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                    ReadValue = "00001"
                }
            };

            //_mockMeterReadingValidator
            //    .Setup(x => x.ValidateAsync(It.IsAny<MeterReading>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(new ValidationResult { Errors = [new()] });

            //var accounts = new List<Account>
            //{
            //    new Account
            //    {
            //        FirstName = "Name",
            //        SurnameName = "Second",
            //        Id = 1
            //    }
            //};

            //var mockDbSetAccounts = accounts.AsQueryable().BuildMockDbSet();
            //_mockApplicationDbContext.Setup(x => x.Accounts).Returns(mockDbSetAccounts.Object);

            // Act
            var result = await _service.ImportMultipleAsync(imports);

            // Assert
            Assert.IsTrue(false);
        }
    }
}