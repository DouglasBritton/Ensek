using CsvHelper;
using DataAccess.Entities;
using ExcelDataReader;
using System.Globalization;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.MeterReadings
{
    public class MeterReadingFileUploadsProcess : IMeterReadingFileUploadsProcess
    {
        public (List<MeterReading> ValidEntries, int NumberOfProcessedEntries) Process(IFormFile file)
        {
            List<FileMeterReadingEntry> entries = file.ContentType switch
            {
                "text/csv" => ReadCsvFile(file),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => ReadExcelFile(file),
                _ => throw new ArgumentException("Unsupported file type."),
            };

            return (GetValidEntries(entries), entries.Count);
        }

        private class FileMeterReadingEntry
        {
            public string AccountId { get; set; }
            public string MeterReadingDateTime { get; set; }
            public string MeterReadValue { get; set; }
        }

        private static List<FileMeterReadingEntry> ReadCsvFile(IFormFile file)
        {
            var entries = new List<FileMeterReadingEntry>();
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        entries = csvReader.GetRecords<FileMeterReadingEntry>().ToList();
                    }
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

        private List<MeterReading> GetValidEntries(List<FileMeterReadingEntry> entries)
        {
            var validEntries = new List<MeterReading>();

            foreach (var entry in entries)
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
                    ReadValue = string.Format("{0:00000}", meterReadValue)
                });
            }

            return validEntries;
        }
    }
}
