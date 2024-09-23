using MediatR;
using ErrorOr;
using App.Application.Customers.Commands;
using App.Domain.Primitives;
using App.Domain.Customers;
using App.Domain.ValueObjects;

namespace App.Application.Customers.Handlers
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
            if (!Guid.TryParse(command.CustomerId, out var guidId))
            {
                return Error.Validation("Customer.Id", "Invalid customer ID");
            }

            var customerId = new CustomerId(guidId);

            if (!await _customerRepository.IsExistAsync(customerId))
            {
                return Error.NotFound("Customer", "Customer not found");
            }

            if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                return Error.Validation("Customer.PhoneNumber", "PhoneNumber has not valid format");
            }

            if (Email.Create(command.Email) is not Email email)
            {
                return Error.Validation("Customer.Email", "Email has not valid format");
            }

            if (Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address)
            {
                return Error.Validation("Customer.Address", "Address has not valid format");
            }

            Customer customer = new Customer(
               customerId,
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
