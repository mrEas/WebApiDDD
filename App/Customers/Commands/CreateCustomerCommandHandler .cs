using App.Domain.Customers;
using App.Domain.Primitives;
using App.Domain.ValueObjects;
using MediatR;

namespace App.Application.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<Unit> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
            {
                throw new ArgumentNullException(nameof(phoneNumber));
            }

            if (Email.Create(command.Email) is not Email email)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address)
            {
                throw new ArgumentNullException(nameof(address));
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
