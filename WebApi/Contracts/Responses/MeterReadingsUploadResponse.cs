namespace WebApi.Contracts.Responses
{
    public class MeterReadingsUploadResponse
    {
        public required int ReadingsAddedSuccessfully { get; init; }
        public required int ReadingsAddedFailed { get; init; }
    }
}
