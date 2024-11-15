using DataAccess.Entities;
using ExcelDataReader;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Responses;
using WebApi.Contracts.Services;

namespace WebApi.MeterReadings.Endpoints
{
    public class MeterReadingUploadsEndpoint :
        Endpoint<MeterReadingsUploadRequest, Results<Ok<MeterReadingsUploadResponse>, BadRequest>>
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingUploadsEndpoint(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        public override void Configure()
        {
            Post("/api/meter-reading-uploads");
            AllowAnonymous();
            AllowFileUploads();
        }

        public override async Task<Results<Ok<MeterReadingsUploadResponse>, BadRequest>>
            ExecuteAsync(MeterReadingsUploadRequest req, CancellationToken ct)
        {
            //TODO check file is sent and check file accepted type
            //if(Files.Count == 0)
            //{
            //    return (Results<Ok<MeterReadingsUploadResponse>, BadRequest>)Results.BadRequest("No file provided");
            //}

            var validEntries = await ReadFileAsync(Files[0]);
            var result = await _meterReadingService.CreateMultipleAsync(validEntries);




            //TODO fix return types
            return (Results<Ok<MeterReadingsUploadResponse>, BadRequest>)result.Match(
                success => Results.Ok(new MeterReadingsUploadResponse { ReadingsAddedSuccessfully = success.Count, ReadingsAddedFailed = 5 - success.Count }),
                failed => Results.BadRequest(failed.Message));
        }

        //TODO change to AddMeterReadingModel
        private static async Task<List<MeterReading>> ReadFileAsync(IFormFile file)
        {
            var list = new List<MeterReading>();
            var numberOfEntries = -1;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (numberOfEntries == -1)
                        {
                            numberOfEntries++;
                            continue;
                        }

                        if (!int.TryParse(reader.GetValue(0)?.ToString(), out int accountIdCell))
                        {
                            numberOfEntries++;
                            continue;
                        }

                        if (!DateTime.TryParse(reader.GetValue(1)?.ToString(), out DateTime readingDateTimeCell))
                        {
                            numberOfEntries++;
                            continue;
                        }

                        if (!int.TryParse(reader.GetValue(2)?.ToString(), out int readValue))
                        {
                            numberOfEntries++;
                            continue;
                        }

                        if (readValue > 99999 || readValue < 0)
                        {
                            numberOfEntries++;
                            continue;
                        }

                        list.Add(new MeterReading { AccountId = accountIdCell, ReadingDateTime = readingDateTimeCell, ReadValue = string.Format("{0:00000}", readValue) });

                        numberOfEntries++;
                    }
                }
            }

            return list;
        }
    }
}
