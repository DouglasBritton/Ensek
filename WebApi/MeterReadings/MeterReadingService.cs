using DataAccess;
using DataAccess.Entities;
using FluentValidation;
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

        public async Task<int> ImportMultipleAsync(List<MeterReading> meterReadings)
        {
            foreach (var meterReading in meterReadings)
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

                var newestMeterReadingInDb = await _dbContext.MeterReadings
                    .Where(x => x.AccountId == meterReading.AccountId)
                    .OrderByDescending(x => x.ReadingDateTime)
                    .FirstOrDefaultAsync();
                if (newestMeterReadingInDb != null && newestMeterReadingInDb.ReadingDateTime >= meterReading.ReadingDateTime)
                {
                    continue;
                }

                await _dbContext.MeterReadings.AddAsync(meterReading);
            }

            return await _dbContext.SaveChangesAsync();
        }
    }
}
