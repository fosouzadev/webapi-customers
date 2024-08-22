using AutoFixture;
using FoSouzaDev.Customers.Domain.ValueObjects;
using System.Net.Mail;

namespace FoSouzaDev.Customers.CommonTests;

public abstract class BaseTest
{
    protected Fixture Fixture { get; private set; }

    protected DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;

    protected string ValidEmail => Fixture.Create<MailAddress>().Address;

    protected BaseTest()
    {
        this.Fixture = new();

        this.Fixture.Customize<BirthDate>(a => a.FromFactory(() => new BirthDate(ValidBirthDate)));
        this.Fixture.Customize<Email>(a => a.FromFactory(() => new Email(ValidEmail)));
    }
}