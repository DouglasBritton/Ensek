using DataAccess.Entities;
using LanguageExt.Common;

namespace WebApi.MeterReadings.Interfaces
{
    public interface IMeterReadingService
    {
        /// <summary>
        /// Adds the valid meter reading into the database.
        /// </summary>
        /// <param name="meterReadings">A list of meter readings.</param>
        /// <returns>The meter readings that were successfully added to the database.</returns>
        Task<List<MeterReading>> ImportMultipleAsync(List<MeterReading> meterReadings);
    }
}
