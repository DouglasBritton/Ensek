using DataAccess.Entities;
using WebApi.Contracts.Responses;

namespace WebApi.Contracts
{
    public static class ResponseMapping
    {
        public static MeterReadingsUploadResponse MapToMeterReadingsUploadResponse(this List<MeterReading> meterReadings, int numberOfTotalEntries)
        {
            return new MeterReadingsUploadResponse
            {
                ReadingsAddedSuccessfully = meterReadings.Count,
                ReadingsAddedFailed = numberOfTotalEntries - meterReadings.Count
            };
        }            
    }
}
