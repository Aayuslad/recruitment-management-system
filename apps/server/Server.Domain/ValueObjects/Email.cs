using System.Text.RegularExpressions;

using Server.Core.ValueObjects;
using Server.Domain.Exceptions;

namespace Server.Domain.ValueObjects
{
    /// <summary>
    /// Email value object with validation.
    /// </summary>
    public sealed class Email : ValueObject
    {
        private Email(string address)
        {
            Address = address;
        }

        public string Address { get; private set; }

        private static readonly Regex _emailRegex =
                    new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email cannot be null or empty.");

            if (!_emailRegex.IsMatch(email))
                throw new DomainException("Invalid email format.");

            return new Email(email);
        }

        /// <summary>
        /// Should not be used in general !
        /// </summary>
        public static Email? SafeCreate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            if (!_emailRegex.IsMatch(email))
                return null;

            return new Email(email);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Address.ToLowerInvariant();
        }

        public override string ToString() => Address;
    }
}