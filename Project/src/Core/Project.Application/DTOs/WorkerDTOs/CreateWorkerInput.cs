using FluentValidation;

namespace Project.Application.DTOs.WorkerDTOs
{
    public record CreateWorkerInput
    {
        public string FinCode { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int DistrictId { get; set; }
    }
    public class WorkerCreateDTOValidator : AbstractValidator<CreateWorkerInput>
    {
        public WorkerCreateDTOValidator()
        {

            RuleFor(w => w.FinCode)
          .NotEmpty().WithMessage("FinCode is required")
          .Length(7).WithMessage("FinCode must be 7 characters long")
          .Matches("^[a-zA-Z0-9]{7}$").WithMessage("FinCode must contain only letters and digits");
            RuleFor(w => w.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MinimumLength(3).WithMessage("FullName must be at least 3 characters long");
            RuleFor(w => w.BirthDate)
                .NotEmpty().WithMessage("BirthDate is required")
                .LessThan(DateTime.Now).WithMessage("BirthDate must be in the past");
            RuleFor(w => w.DistrictId)
                .GreaterThan(0).WithMessage("DistrictId must be greater than 0");
        }
    }

}
