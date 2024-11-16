using DataAccess.Entities;

namespace WebApi.MeterReadings
{
    public class FileProcessedModel
    {
        public required List<MeterReading> ValidEntries { get; set; }
        public required int TotalProcessedEntries { get; set; }

    }
}
