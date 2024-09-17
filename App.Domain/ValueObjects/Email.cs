using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public partial record Email
    {
        private const string Pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        private Email(string value) => Value = value;
        public string Value { get; init; }

        public static Email? Create(string value)
        {
            if (string.IsNullOrEmpty(value)
                || !EmailRegex().IsMatch(value))
            {
                return null;
            }

            return new Email(value);
        }

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}
