using FastEndpoints;
using FluentValidation;
using WebApi.Contracts.Requests;

namespace WebApi.Contracts.Validators
{
    public class MeterReadingUploadsRequestValidator : Validator<MeterReadingUploadsRequest>
    {
        public MeterReadingUploadsRequestValidator()
        {
            RuleFor(x => x.File).NotEmpty().WithMessage("A file must be sent.");
            RuleFor(x => x.File.ContentType).Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet").WithMessage("A file of xlsx type must be used.");
        }
    }
}
