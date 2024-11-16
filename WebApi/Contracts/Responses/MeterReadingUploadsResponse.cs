namespace WebApi.Contracts.Responses
{
    public class MeterReadingUploadsResponse
    {
        public required int ReadingsAddedSuccessfully { get; init; }
        public required int ReadingsAddedFailed { get; init; }
    }
}
