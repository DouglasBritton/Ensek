using DataAccess.Entities;
using ExcelDataReader;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Contracts;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Responses;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.MeterReadings.Endpoints
{
    public class MeterReadingUploadsEndpoint :
        Endpoint<MeterReadingUploadsRequest, Ok<MeterReadingsUploadResponse>>
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

        public override async Task<Ok<MeterReadingsUploadResponse>>
            ExecuteAsync(MeterReadingUploadsRequest req, CancellationToken ct)
        {
            var validEntries = ReadFile(req.File, out int numberOfTotalEntries);

            var result = await _meterReadingService.ImportMultipleAsync(validEntries);

            return TypedResults.Ok(result.MapToMeterReadingsUploadResponse(numberOfTotalEntries));
        }

        private static List<MeterReading> ReadFile(IFormFile file, out int numberOfTotalEntries)
        {
            var list = new List<MeterReading>();
            numberOfTotalEntries = -1;

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (numberOfTotalEntries == -1)
                        {
                            numberOfTotalEntries++;
                            continue;
                        }

                        if (!int.TryParse(reader.GetValue(0)?.ToString(), out int accountIdCell))
                        {
                            numberOfTotalEntries++;
                            continue;
                        }

                        if (!DateTime.TryParse(reader.GetValue(1)?.ToString(), out DateTime readingDateTimeCell))
                        {
                            numberOfTotalEntries++;
                            continue;
                        }

                        if (!int.TryParse(reader.GetValue(2)?.ToString(), out int readValue))
                        {
                            numberOfTotalEntries++;
                            continue;
                        }

                        if (readValue > 99999 || readValue < 0)
                        {
                            numberOfTotalEntries++;
                            continue;
                        }

                        list.Add(new MeterReading { AccountId = accountIdCell, ReadingDateTime = readingDateTimeCell, ReadValue = string.Format("{0:00000}", readValue) });

                        numberOfTotalEntries++;
                    }
                }
            }

            return list;
        }
    }
}
