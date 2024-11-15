using DataAccess.Entities;
using FluentValidation;

namespace WebApi.MeterReadings
{
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        public MeterReadingValidator()
        {
            //TODO - "NNNNN" format, AccountId must exist in db. can't duplicate entries AccountId and readingdate match?
            //When an account has an existing read, ensure the new read isn’t older than the existing read
            RuleFor(x => x.ReadValue).NotEmpty();
        }
    }
}
