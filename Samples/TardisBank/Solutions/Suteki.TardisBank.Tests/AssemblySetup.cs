namespace Suteki.TardisBank.Tests
{
    using NUnit.Framework;

    [SetUpFixture]
    public class AssemblySetup
    {
         [SetUp]
        public void RunBeforeAllTests()
         {
             log4net.Config.XmlConfigurator.Configure();
             ServiceLocatorInitializer.Init();
         }
    }
}