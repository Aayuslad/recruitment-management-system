using System.Text.RegularExpressions;

using Server.Core.Results;
using Server.Core.ValueObjects;

namespace Server.Domain.ValueObjects

{
    /// <summary>
    /// contact number value object with validation, "+{country}{number}" format
    /// </summary>
    public sealed class ContactNumber : ValueObject
    {
        private ContactNumber(string number)
        {
            Number = number;
        }

        public string Number { get; private set; }

        private static readonly Regex _phoneRegex =
                    new(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);

        public static Result<ContactNumber> Create(string number)
        {
            number = Regex.Replace(number, @"[\s\-()]", "");

            if (string.IsNullOrWhiteSpace(number))
                return Result<ContactNumber>.Failure("Contact number cannot be empty.");
            if (!_phoneRegex.IsMatch(number))
                return Result<ContactNumber>.Failure("Contact number format is invalid.");

            return Result<ContactNumber>.Success(new ContactNumber(number));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Number;
        }

        public override string ToString() => Number;
    }
}