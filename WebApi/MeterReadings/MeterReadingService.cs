using DataAccess;
using DataAccess.Entities;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using WebApi.MeterReadings.Interfaces;

namespace WebApi.MeterReadings
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IValidator<MeterReading> _validator;
        private readonly ApplicationDbContext _dbContext;

        public MeterReadingService(IValidator<MeterReading> validator, ApplicationDbContext dbContext)
        {
            _validator = validator;
            _dbContext = dbContext;
        }

        public async Task<List<MeterReading>> ImportMultipleAsync(List<MeterReading> meterReadings)
        {
            foreach(var meterReading in meterReadings)
            {
                var validationResult = await _validator.ValidateAsync(meterReading);
                if (!validationResult.IsValid)
                {
                    continue;
                }

                var account = _dbContext.Accounts.Find(meterReading.AccountId);
                if (account is null)
                {
                    continue;
                }

                var newestReadingDateTime = account?.MeterReadings?.Max(meterReading => meterReading.ReadingDateTime);
                if(newestReadingDateTime >= meterReading.ReadingDateTime)
                {
                    continue;
                }

                await _dbContext.MeterReadings.AddAsync(meterReading);
            }

            await _dbContext.SaveChangesAsync();

            return meterReadings;
        }
    }
}
