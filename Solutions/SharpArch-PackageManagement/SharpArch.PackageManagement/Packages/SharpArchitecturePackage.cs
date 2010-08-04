namespace SharpArch.PackageManagement.Packages
{
    using System;
    using System.IO;

    public static class SharpArchitecturePackage
    {
        public static string Location 
        { 
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                    @"Sharp-Architecture\Package-Repository\Sharp-Architecture.pkg");
            }
        }
    }
}