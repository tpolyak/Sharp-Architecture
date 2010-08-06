namespace SharpArch.PackageManagement.Contracts.Packager.Builders
{
    #region Using Directives

    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    public interface IPackageBuilder
    {
        Package Build(string path, IPackageMetaData packageMetaData);
    }
}