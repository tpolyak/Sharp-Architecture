namespace SharpArch.PackageManagement.Domain.Packages
{
    #region Using Directives

    using SharpArch.PackageManagement.Contracts.Packages;

    #endregion

    public class PackageMetaData : IPackageMetaData
    {
        public string Author { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }
    }
}