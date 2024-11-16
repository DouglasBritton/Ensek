
using DataAccess.Entities;

namespace WebApi.MeterReadings.Interfaces
{
    public interface IMeterReadingFileUploadsProcess
    {
        (List<MeterReading> ValidEntries, int NumberOfProcessedEntries) Process(IFormFile file);
    }
}
