using FluentValidation;
using LearningDDD.Api.Dtos.Group;

namespace LearningDDD.Api.Validators
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroup>
    {
        public UpdateGroupValidator()
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
