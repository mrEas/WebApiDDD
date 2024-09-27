using MediatR;
using ErrorOr;
using App.Domain.Primitives;
using App.Domain.Customers;
using App.Domain.ValueObjects;
using App.Domain.DomainErrors;

namespace App.Application.Customers.Commands
{
    public class UpdateCustomerCommandHadndler : IRequestHandler<UpdateCustomerCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHadndler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            if (!await _customerRepository.IsExistAsync(new CustomerId(command.CustomerId)))
            {
                return CustomerErrors.CustomerNotFound;
            }

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
               new CustomerId(command.CustomerId),
               command.FirstName,
               command.LastName,
               email,
               phoneNumber,
               address);

            _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
