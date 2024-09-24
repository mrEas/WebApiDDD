using App.Domain.Customers;
using App.Domain.DomainErrors;
using App.Domain.Primitives;
using App.Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            var email = Email.Create(command.Email);
            var address = Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode);

            if (phoneNumber is not PhoneNumber)
            {
                errors.Add(CustomerErrors.PhoneNumberIsNotValid);
            }

            if (email is not Email)
            {
                errors.Add(CustomerErrors.EmailIssNotValid);
            }

            if (address is not Address)
            {
                errors.Add(CustomerErrors.AddressIsNotValid);
            }

            if (errors.Any())
            {
                return errors;
            }

            Customer customer = new Customer(
                new CustomerId(Guid.NewGuid()),
                command.FirstName,
                command.LastName,
                email,
                phoneNumber,
                address,
                true);

            _customerRepository.Add(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
