using AutoFixture;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.CommonTests;

public abstract class BaseTest
{
    protected Fixture Fixture { get; private set; }

    protected BaseTest()
    {
        Fixture = new();

        Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidDataGenerator.ValidBirthDate)));
        Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidDataGenerator.ValidEmail)));
    }
}