namespace SharpArch.PackageManagement.Contracts.Packager.Builders
{
    using SharpArch.PackageManagement.Domain.Packages;

    public interface IPackageBuilder
    {
        Package Build(string path);
    }
}