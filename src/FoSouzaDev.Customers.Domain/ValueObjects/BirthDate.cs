using FoSouzaDev.Customers.Domain.Exceptions;

namespace FoSouzaDev.Customers.Domain.ValueObjects;

public sealed record BirthDate
{
    public DateTime Date { get; private set; }

    public BirthDate(DateTime date)
    {
        int age = CalculateAge(date.Date);

        if (age < 18 || age > 100)
            throw new ValidateException("Invalid age.");

        this.Date = date.Date;
    }

    private static int CalculateAge(DateTime birthDate)
    {
        DateTime now = DateTime.Now.Date;

        int age = now.Year - birthDate.Year;
        if (now.AddYears(-age) < birthDate)
            --age;

        return age;
    }
}