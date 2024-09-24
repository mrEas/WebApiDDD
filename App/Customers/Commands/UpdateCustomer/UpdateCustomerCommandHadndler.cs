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

            var phoneNumber = PhoneNumber.Create(command.PhoneNumber);
            var email = Email.Create(command.Email);
            var address = Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode);

            if (phoneNumber is not PhoneNumber)
            {
                errors.Add(CustomerErrors.PhoneNumberIsNotValid);
            }

            if (email is not Email)
            {
                errors.Add(CustomerErrors.PhoneNumberIsNotValid);
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
