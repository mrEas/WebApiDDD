﻿using App.Application.Customers.Queries;
using App.Domain.Customers;
using MediatR;

namespace App.Application.Customers.Handlers
{
    public class IsCustomerExistQueryHandler : IRequestHandler<IsCustomerExistQuery, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        public IsCustomerExistQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task<bool> Handle(IsCustomerExistQuery request, CancellationToken cancellationToken)
        {
            return _customerRepository.IsExistAsync(request.CustomerId);
        }
    }
}