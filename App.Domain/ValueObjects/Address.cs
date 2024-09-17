using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public record Address
    {
        public string Country { get; init; }
        public string Line1 { get; init; }
        public string Line2 { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ZipCode { get; init; }

        public Address(string country, string line1, string line2, string city, string state, string zipCode)
        {
            Country = country;
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public static Address? Create(string country, string line1, string line2, string city, string state, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(country)
                || string.IsNullOrWhiteSpace(line1)
                || string.IsNullOrWhiteSpace(line2)
                || string.IsNullOrWhiteSpace(city)
                || string.IsNullOrWhiteSpace(zipCode)
                || string.IsNullOrWhiteSpace(state))
            {
                return null;
            }

            return new Address(country, line1, line2, city, state, zipCode);
        }
    }
}
