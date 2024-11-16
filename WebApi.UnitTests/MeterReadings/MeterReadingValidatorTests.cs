using DataAccess.Entities;
using WebApi.MeterReadings;

namespace WebApi.UnitTests.MeterReadings
{
    [TestClass]
    public class MeterReadingValidatorTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private MeterReadingValidator _validator;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void Setup()
        {
            _validator = new MeterReadingValidator();
        }

        [TestMethod]
        //Valid
        [DataRow(true, 1, "00000")]
        [DataRow(true, 1, "00025")]
        [DataRow(true, 1, "99999")]
        [DataRow(true, 10, "00000")]
        [DataRow(true, 10, "00025")]
        [DataRow(true, 10, "99999")]
        //Invalid
        [DataRow(false, -10, "00001")]
        [DataRow(false, 0, "00001")]
        [DataRow(false, 10, "002.5")]
        [DataRow(false, 10, "-0250")]
        [DataRow(false, 10, "50")]
        public void MeterReadingValidator_OnlyAllowsValid(bool expectedValidation,
            int accountId, string readValue)
        {
            // Arrange
            var entity = new MeterReading
            {
                AccountId = accountId,
                ReadingDateTime = new DateTime(2024, 11, 16, 0, 0, 0, DateTimeKind.Utc),
                ReadValue = readValue
            };

            // Act
            var result = _validator.Validate(entity);

            // Assert
            Assert.AreEqual(expectedValidation, result.IsValid);
        }
    }
}