using FastEndpoints;
using FluentValidation;
using WebApi.Contracts.Requests;

namespace WebApi.Contracts.Validators
{
    public class MeterReadingUploadsRequestValidator : Validator<MeterReadingUploadsRequest>
    {
        public MeterReadingUploadsRequestValidator()
        {
            var acceptedFileTypes = new List<string>() { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv" };

            RuleFor(x => x.File.ContentType).Must(x => acceptedFileTypes.Contains(x)).WithMessage("The file type provided is not supported.");
        }
    }
}
