using FoSouzaDev.Customers.Domain.Exceptions;

namespace FoSouzaDev.Customers.Domain.ValueObjects;

public sealed record FullName
{
    public string Name { get; private set; }
    public string LastName { get; private set; }

    public FullName(string? name, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            throw new ValidateException("Invalid name.");

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 3)
            throw new ValidateException("Invalid last name.");

        this.Name = name;
        this.LastName = lastName;
    }
}