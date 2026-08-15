using FluentValidation;

namespace Users.Dto.Request;

internal record UserCreateDto(
    string FirstName,
    string LastName,
    string Mobile
);


internal class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        // Example rules
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Mobile)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(@"^[0-9]{10,15}$").WithMessage("Mobile must contain only numbers and be 10-15 digits long.");
    }
}