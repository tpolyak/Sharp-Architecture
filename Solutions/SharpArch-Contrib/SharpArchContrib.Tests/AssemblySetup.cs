using System.IO;
using log4net.Config;
using NUnit.Framework;
using Tests.Configuration;

namespace Tests {
    [SetUpFixture]
    public class AssemblySetup {
        [SetUp]
        public void SetUp() {
            InitializeDirectories();
            InitializeLog4Net();
            InitalizeServiceLocator();
        }

        private void InitalizeServiceLocator() {
            ServiceLocatorInitializer.Init();
        }

        private void InitializeLog4Net() {
            XmlConfigurator.Configure();
        }

        private void InitializeDirectories() {
            if (Directory.Exists(Config.TestDataDir)) {
                Directory.Delete(Config.TestDataDir, true);
            }
            Directory.CreateDirectory(Config.TestDataDir);
        }
    }
}