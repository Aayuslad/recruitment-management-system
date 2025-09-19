using System.Text.RegularExpressions;

using Server.Core.Results;
using Server.Core.ValueObjects;

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

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<Email>.Failure("Email cannot be empty.");

            if (!_emailRegex.IsMatch(email))
                return Result<Email>.Failure("Email format is invalid.");

            return Result<Email>.Success(new Email(email));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Address.ToLowerInvariant();
        }

        public override string ToString() => Address;
    }
}