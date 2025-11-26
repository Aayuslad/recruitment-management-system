using PhoneNumbers;

using Server.Core.ValueObjects;
using Server.Domain.Exeptions;

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

        public static ContactNumber Create(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new DomainExeption("Contact number cannot be empty.");
            }

            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var parsedNumber = phoneNumberUtil.Parse(number, "IN");

                if (!phoneNumberUtil.IsValidNumber(parsedNumber))
                {
                    throw new DomainExeption("Invalid contact number format.");
                }

                string e164Number = phoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.E164);

                return new ContactNumber(e164Number);
            }
            catch (Exception)
            {
                throw new DomainExeption("Invalid contact number format.");
            }
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Number;
        }

        public override string ToString() => Number;
    }
}