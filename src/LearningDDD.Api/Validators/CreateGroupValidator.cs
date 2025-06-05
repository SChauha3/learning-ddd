using FluentValidation;
using LearningDDD.Api.Dtos.Group;

namespace LearningDDD.Api.Validators
{
    public class CreateGroupValidator : AbstractValidator<CreateGroup>
    {
        public CreateGroupValidator()
        {
            RuleFor(g => g.Capacity)
                .GreaterThan(0)
                .NotNull();

            RuleFor(g => g.Name)
                .NotEmpty()
                .NotNull();
        }
    }
}
