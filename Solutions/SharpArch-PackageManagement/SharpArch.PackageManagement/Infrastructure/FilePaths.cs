namespace SharpArch.PackageManagement.Infrastructure
{
    #region Using Directives

    using System;
    using System.IO;

    #endregion

    public static class FilePaths
    {
        public static string PackageRepository 
        { 
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"Sharp-Architecture\Package-Repository\");
            }
        }

        public static string TemporaryPackageRepository
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"Sharp-Architecture\Temp-Package-Repository\");
            }
        }
    }
}