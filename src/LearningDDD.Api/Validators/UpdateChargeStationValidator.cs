using FluentValidation;
using LearningDDD.Api.Dtos.ChargeStation;

namespace LearningDDD.Api.Validators
{
    public class UpdateChargeStationValidator : AbstractValidator<UpdateChargeStation>
    {
        public UpdateChargeStationValidator()
        {
            RuleFor(cs => cs.Name)
                .NotEmpty()
                .NotNull();
            
            RuleFor(cs => cs.GroupId)
                .NotEmpty()
                .NotNull()
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("GroupId must be a valid GUID.");

            //RuleFor(cs => cs.Connectors)
            //    .NotNull()
            //    .Must(connectors => connectors.Count >= 1 && connectors.Count <= 5)
            //    .WithMessage("A Charge Station must have between 1 and 5 connectors.")
            //    .Must(connectors => connectors.Select(c => c.ChargeStationContextId).Distinct().Count() == connectors.Count)
            //    .WithMessage("Connector Ids must be unique within the context of a charge station with (possible range of values from 1 to 5).");

            //RuleForEach(cs => cs.Connectors)
            //    .ChildRules(connector =>
            //    {
            //        connector.RuleFor(c => c.MaxCurrent)
            //        .GreaterThan(0)
            //        .WithMessage("Max current must be greater than 0.");

            //        connector.RuleFor(c => c.ChargeStationContextId)
            //        .InclusiveBetween(1, 5)
            //        .WithMessage("Id must be between 1 and 5.");
            //    });
        }
    }
}