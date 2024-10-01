using System.Text.RegularExpressions;

namespace App.Domain.ValueObjects
{
    public partial record PhoneNumber
    {
        private const int DefaultLength = 9;
        private const string Pattern = @"^(?:-*\d-*){8}$";

        private PhoneNumber(string value) => Value = value;
        public string Value { get; init; }

        // Only to support ef core query.
        public static explicit operator string(PhoneNumber phone) => phone.Value;
        public static PhoneNumber? Create(string value)
        {
            if (string.IsNullOrEmpty(value)
            || value.Length != DefaultLength
            || !PhoneNumberRegex().IsMatch(value))
            {
                return null;
            }

            return new PhoneNumber(value);
        }

        [GeneratedRegex(Pattern)]
        private static partial Regex PhoneNumberRegex();
    }
}
