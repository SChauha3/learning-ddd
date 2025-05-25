using FluentValidation;
using SmartCharging.Api.Dtos.Connector;

namespace SmartCharging.Api.Validators
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