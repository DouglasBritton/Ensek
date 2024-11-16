using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Responses;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.MeterReadings.Endpoints
{
    public class MeterReadingUploadsEndpoint :
        Endpoint<MeterReadingUploadsRequest, Ok<MeterReadingUploadsResponse>>
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly IMeterReadingFileUploadsProcess _fileUploadProcess;

        public MeterReadingUploadsEndpoint(IMeterReadingService meterReadingService,
            IMeterReadingFileUploadsProcess fileUploadProcess)
        {
            _meterReadingService = meterReadingService;
            _fileUploadProcess = fileUploadProcess;
        }

        public override void Configure()
        {
            Post("/api/meter-reading-uploads");
            AllowAnonymous();
            AllowFileUploads();
        }

        public override async Task<Ok<MeterReadingUploadsResponse>>
            ExecuteAsync(MeterReadingUploadsRequest req, CancellationToken ct)
        {
            var (validEntries, numberOfProcessedEntries) = _fileUploadProcess.Process(req.File);

            var result = await _meterReadingService.ImportMultipleAsync(validEntries);

            var response = new MeterReadingUploadsResponse
            {
                ReadingsAddedSuccessfully = result,
                ReadingsAddedFailed = numberOfProcessedEntries - result
            };

            return TypedResults.Ok(response);
        }
    }
}
