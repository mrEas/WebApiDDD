﻿using App.Application.Customers.Queries;
using App.Application.Customers.QueryObjects;
using App.Domain.Customers;
using MediatR; 

namespace App.Application.Customers.Handlers
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto(
                 c.Id.ToString(),
                 c.FirstName,
                 c.LastName,
                 c.Email.Value,
                 c.PhoneNumber.Value,
                 c.Address.Country,
                 c.Address.Line1,
                 c.Address.Line2,
                 c.Address.City,
                 c.Address.State,
                 c.Address.ZipCode,
                 c.IsActive)
            ).ToList();
        }

    }
}