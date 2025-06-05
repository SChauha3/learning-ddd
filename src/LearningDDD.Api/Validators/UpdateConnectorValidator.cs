using FluentValidation;
using LearningDDD.Api.Dtos.Connector;

namespace LearningDDD.Api.Validators
{
    public class UpdateConnectorValidator : AbstractValidator<UpdateConnector>
    {
        public UpdateConnectorValidator()
        {
            RuleFor(c => c.MaxCurrent)
                .GreaterThan(0);
        }
    }
}