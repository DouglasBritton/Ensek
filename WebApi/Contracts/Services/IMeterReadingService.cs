using DataAccess.Entities;
using LanguageExt.Common;

namespace WebApi.Contracts.Services
{
    public interface IMeterReadingService
    {
        /// <summary>
        /// Adds the valid meter reading into the database.
        /// </summary>
        /// <param name="meterReadings">A list of meter readings.</param>
        /// <returns>The meter readings that were successfully added to the database.</returns>
        Task<Result<List<MeterReading>>> CreateMultipleAsync(List<MeterReading> meterReadings);
    }
}
