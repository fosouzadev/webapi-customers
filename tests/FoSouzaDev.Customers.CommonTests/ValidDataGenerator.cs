using AutoFixture;
using System.Net.Mail;

namespace FoSouzaDev.Customers.CommonTests
{
    public static class ValidDataGenerator
    {
        public static DateTime ValidBirthDate => DateTime.Now.AddYears(-18).Date;

        public static string ValidEmail => new Fixture().Create<MailAddress>().Address;
    }
}