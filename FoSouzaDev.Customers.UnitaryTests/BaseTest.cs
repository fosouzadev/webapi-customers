using AutoFixture;

namespace FoSouzaDev.Customers.UnitaryTests
{
    public abstract class BaseTest
    {
        protected Fixture Fixture { get; private set; }

        protected BaseTest()
        {
            this.Fixture = new();
        }
    }
}