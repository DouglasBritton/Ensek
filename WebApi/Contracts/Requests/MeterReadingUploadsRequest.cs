namespace WebApi.Contracts.Requests
{
    public class MeterReadingUploadsRequest
    {
        public required IFormFile File { get; set; }
    }
}
