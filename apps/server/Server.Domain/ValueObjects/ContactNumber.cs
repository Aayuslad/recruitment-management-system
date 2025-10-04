using PhoneNumbers;

using Server.Core.Results;
using Server.Core.ValueObjects;

namespace Server.Domain.ValueObjects

{
    /// <summary>
    /// Contact number value object with strict E.164 normalization.
    /// Always stored in "+{country}{national}" format.
    /// </summary>
    public sealed class ContactNumber : ValueObject
    {
        private ContactNumber(string number)
        {
            Number = number;
        }

        public string Number { get; private set; }

        public static Result<ContactNumber> Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return Result<ContactNumber>.Failure("Contact number cannot be empty.", 400);
            }

            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var parsedNumber = phoneNumberUtil.Parse(number, "IN");

                if (!phoneNumberUtil.IsValidNumber(parsedNumber))
                {
                    return Result<ContactNumber>.Failure("Invalid contact number.", 400);
                }

                string e164Number = phoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.E164);

                return Result<ContactNumber>.Success(new ContactNumber(e164Number));
            }
            catch (Exception)
            {
                return Result<ContactNumber>.Failure("Invalid contact number.", 400);
            }
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Number;
        }

        public override string ToString() => Number;

        /// <summary>
        /// Returns the phone number in **national format** according to the local dialing conventions.
        /// Example for +91 9723165858: "09723 165858"
        /// Use this for displaying the number to users in the same country.
        /// </summary>
        /// <returns>Phone number as string in national format.</returns>
        public string ToNationalFormat()
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var proto = phoneUtil.Parse(Number, null);
            return phoneUtil.Format(proto, PhoneNumberFormat.NATIONAL);
        }

        /// <summary>
        /// Returns the phone number in **international format** with country code and separators.
        /// Example for +91 9723165858: "+91 97231 65858"
        /// Use this for displaying the number to foreign users or in global contexts.
        /// </summary>
        public string ToInternationalFormat()
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var proto = phoneUtil.Parse(Number, null);
            return phoneUtil.Format(proto, PhoneNumberFormat.INTERNATIONAL);
        }

        /// <summary>
        /// Gets the **country code** of the phone number.
        /// Example for +91 9723165858: 91
        /// </summary>
        public int CountryCode
        {
            get
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var proto = phoneUtil.Parse(Number, null);
                return proto.CountryCode;
            }
        }

        /// <summary>
        /// Gets the **national number** (local part without country code).
        /// Example for +91 9723165858: 9723165858
        /// </summary>
        public ulong NationalNumber
        {
            get
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var proto = phoneUtil.Parse(Number, null);
                return proto.NationalNumber;
            }
        }
    }
}