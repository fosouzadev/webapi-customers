using AutoFixture;
using FoSouzaDev.Customers.Domain.ValueObjects;

namespace FoSouzaDev.Customers.UnitaryTests
{
    public abstract class BaseTest
    {
        protected Fixture Fixture { get; private set; }

        protected static DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;

        protected static string ValidEmail => "test@test.com";

        protected BaseTest()
        {
            this.Fixture = new();

            this.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidBirthDate)));
            this.Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidEmail)));
        }
    }
}