﻿using ErrorOr;

namespace App.Domain.DomainErrors
{
    public static class CustomerErrors
    {
        public static Error PhoneNumberIsNotValid => Error.Validation("Customer.PhoneNumber", "PhoneNumber has not valid format");
        public static Error EmailIsNotValid => Error.Validation("Customer.Email", "Email has not valid format");
        public static Error AddressIsNotValid => Error.Validation("Customer.Address", "Address has not valid format");
        public static Error CustomerNotFound=> Error.NotFound("Customer", "Customer not found");
        public static Error EmailAlreadyExists => Error.Conflict("Customer.Email", "Email already exists");
        public static Error PhoneAlreadyExists => Error.Conflict("Customer.Phone", "Phone already exists");
    }

}
