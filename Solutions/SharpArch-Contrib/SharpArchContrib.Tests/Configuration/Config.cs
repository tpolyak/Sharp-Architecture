using System.IO;

namespace Tests.Configuration {
    public static class Config {
        public static string TestDataDir {
            get { return Path.GetFullPath("TestData"); }
        }
    }
}