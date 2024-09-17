using App.Domain.Primitives;
using App.Domain.ValueObjects; 

namespace App.Domain.Customers
{
    public sealed class Customer : AggregateRoot
    {
        public CustomerId Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public bool IsActive { get; private set; }

        public Customer(CustomerId id, string firstName, string lastName, Email email = null, PhoneNumber phoneNumber = null, Address address = null, bool isActive = false)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            IsActive = isActive;
        }


    }
}
