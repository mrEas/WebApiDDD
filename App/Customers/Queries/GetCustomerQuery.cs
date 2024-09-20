﻿using App.Domain.Customers;
using MediatR;

namespace App.Application.Customers.Queries
{
    public record GetCustomerQuery(CustomerId CustomerId) : IRequest<Customer?>;
}
