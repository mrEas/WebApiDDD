﻿namespace App.Application.Customers.QueryObjects
{
    public record CustomerDto(
       Guid Id,
       string FirstName,
       string LastName,
       string Email,
       string PhoneNumber,
       string Country,
       string Line1,
       string Line2,
       string City,
       string State,
       string ZipCode,
       bool IsActive);
} 