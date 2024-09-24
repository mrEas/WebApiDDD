using FluentValidation;

namespace App.Application.Customers.Commands
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .MaximumLength(1);

            RuleFor(r => r.LastName)
                .NotNull()
                .MaximumLength(255);

            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .WithName(nameof(CreateCustomerCommand.Email));

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .MaximumLength(9)
                .WithName(customer => "Phone for customer " + customer.FirstName);

            RuleFor(r => r.Country)
               .NotEmpty()
               .MaximumLength(255);

            RuleFor(r => r.Line1)
               .NotEmpty()
               .MaximumLength(255);

            RuleFor(r => r.City)
               .NotEmpty()
               .MaximumLength(255);

            RuleFor(r => r.State)
               .NotEmpty()
               .MaximumLength(255);

            RuleFor(r => r.ZipCode)
               .NotEmpty()
               .MaximumLength(255);
        }
    }
}
