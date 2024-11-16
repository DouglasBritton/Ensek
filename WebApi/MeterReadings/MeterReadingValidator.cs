using DataAccess.Entities;
using FluentValidation;

namespace WebApi.MeterReadings
{
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        public MeterReadingValidator()
        {
            RuleFor(x => x.ReadValue).Length(5).WithMessage("The reading value must be in the form 'NNNNN'.");
            RuleFor(x => x.ReadValue).Must(value => int.TryParse(value, out int result) && result >= 0).WithMessage("The reading value must be a positive number.");
        }
    }
}
