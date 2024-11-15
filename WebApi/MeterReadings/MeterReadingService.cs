using DataAccess;
using DataAccess.Entities;
using FluentValidation;
using LanguageExt.Common;
using WebApi.Contracts.Services;

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

        public async Task<Result<List<MeterReading>>> CreateMultipleAsync(List<MeterReading> meterReadings)
        {
            foreach(var  meterReading in meterReadings)
            {
                var validationResult = await _validator.ValidateAsync(meterReading);
                if (validationResult.IsValid)
                {
                    //TODO check if meter reading has already been added (reading date) or if account has newer meter reading than one being added
                    await _dbContext.MeterReadings.AddAsync(meterReading);
                }
            }

            await _dbContext.SaveChangesAsync();

            return meterReadings;
        }
    }
}
