using CsvHelper;
using DataAccess.Entities;
using ExcelDataReader;
using FastEndpoints;
using LanguageExt;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;
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
            int numberOfTotalEntries;
            List<FileMeterReadingEntry> entries;

            switch (req.File.ContentType)
            {
                case "text/csv":
                    entries = ReadCsvFile(req.File);
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    entries = ReadExcelFile(req.File);
                    break;
                default: throw new ArgumentException("Unsupported file type.");
            }

            var validEntries = GetValidEntries(entries);

            var result = 1;// await _meterReadingService.ImportMultipleAsync(validEntries);

            var response = new MeterReadingsUploadResponse
            {
                ReadingsAddedSuccessfully = result,
                ReadingsAddedFailed = entries.Count - result
            };

            return TypedResults.Ok(response);
        }

        private List<MeterReading> GetValidEntries(List<FileMeterReadingEntry> entries)
        {
            var validEntries = new List<MeterReading>();

            foreach(var entry in entries)
            {
                if (!int.TryParse(entry.AccountId, out int accountId))
                {
                    continue;
                }

                if (!DateTime.TryParse(entry.MeterReadingDateTime, out DateTime readingDateTime))
                {
                    continue;
                }

                if (!int.TryParse(entry.MeterReadValue, out int meterReadValue))
                {
                    continue;
                }

                if (meterReadValue > 99999 || meterReadValue < 0)
                {
                    continue;
                }

                validEntries.Add(new MeterReading
                {
                    AccountId = accountId,
                    ReadingDateTime = readingDateTime,
                    ReadValue = meterReadValue
                });
            }

            return validEntries;
        }

        private class FileMeterReadingEntry
        {
            public string AccountId { get; set; }
            public string MeterReadingDateTime { get; set; }
            public string MeterReadValue { get; set; }
        }

        private List<FileMeterReadingEntry> ReadCsvFile(IFormFile file)
        {
            var entries = new List<FileMeterReadingEntry>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    entries = csvReader.GetRecords<FileMeterReadingEntry>().ToList();
                }
            }

            return entries;
        }

        private static List<FileMeterReadingEntry> ReadExcelFile(IFormFile file)
        {
            var entries = new List<FileMeterReadingEntry>();
            var header = true;

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (header)
                        {
                            header = false;
                            continue;
                        }

                        entries.Add(
                            new FileMeterReadingEntry 
                            { 
                                AccountId = reader.GetValue(0)?.ToString(), 
                                MeterReadingDateTime = reader.GetValue(1)?.ToString(), 
                                MeterReadValue = reader.GetValue(2)?.ToString() 
                            });
                    }
                }
            }

            return entries;
        }
    }
}
