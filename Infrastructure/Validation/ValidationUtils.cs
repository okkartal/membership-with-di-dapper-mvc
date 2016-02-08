using System;
using System.Text.RegularExpressions;

namespace Infrastructure.Validation
{
    public class ValidationUtils
    {
        public static bool PhoneNumberIsValid(String phoneNumber)
        {
            return new Regex(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$").IsMatch(phoneNumber);
        }

        public static bool EmailAddressIsValid(string emailAddress)
        {
            return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(emailAddress);
        }
        public static bool UserNameIsValid(string userName)
        {
            return new Regex(@"^(?=[A-Za-z0-9])(?!.*[._()\[\]-]{2})[A-Za-z0-9._()\[\]-]{3,50}$").IsMatch(userName);
        }

        public static bool StringEmptyIsValid(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
