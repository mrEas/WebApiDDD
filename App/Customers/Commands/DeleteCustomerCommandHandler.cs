using App.Domain.Customers;
using App.Domain.Primitives;
using ErrorOr;
using MediatR;

namespace App.Application.Customers.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<ErrorOr<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if(customer is null)
            {
                return Error.NotFound("Customer.NotFound", "Customer not found.");

            }
            _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
