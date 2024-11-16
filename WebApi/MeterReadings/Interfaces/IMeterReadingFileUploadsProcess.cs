namespace WebApi.MeterReadings.Interfaces
{
    public interface IMeterReadingFileUploadsProcess
    {
        Task<FileProcessedModel> ProcessAsync(IFormFile file);
    }
}
