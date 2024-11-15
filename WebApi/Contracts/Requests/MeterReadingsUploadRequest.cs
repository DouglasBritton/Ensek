namespace WebApi.Contracts.Requests
{
    public class MeterReadingsUploadRequest
    {
        public required IFormFile File { get; set; }
    }
}
