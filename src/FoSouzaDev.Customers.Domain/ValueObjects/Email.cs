using FoSouzaDev.Customers.Domain.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FoSouzaDev.Customers.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; private set; }

    public Email(string email)
    {
        if (IsValidEmail(email) == false)
            throw new ValidateException("Invalid email.");

        this.Value = email;
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            static string DomainMapper(Match match)
            {
                IdnMapping idn = new();
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }

            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (Exception)
        {
            return false;
        }
    }
}