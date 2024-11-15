using App.Domain.Customers;
using App.Domain.DomainErrors;
using App.Domain.Primitives;
using App.Domain.ValueObjects;
using ErrorOr;
using MediatR;
using System.Diagnostics;

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
            if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                return CustomerErrors.PhoneNumberIsNotValid;
            }

            if (Email.Create(command.Email) is not Email email)
            {
                return CustomerErrors.EmailIsNotValid;
            }

            if (Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address)
            {
                return CustomerErrors.AddressIsNotValid;
            }

            if (await _customerRepository.IsExistsByEmailAsync(email))
            {
                return CustomerErrors.EmailAlreadyExists;
            }

            if (await _customerRepository.IsExistsByPhoneAsync(phoneNumber))
            {
                return CustomerErrors.PhoneAlreadyExists;
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
