namespace SharpArch.PackageManagement.Contracts.Packager.Builders
{
    using SharpArch.PackageManagement.Domain.Packages;

    public interface IClonePackageBuilder
    {
        Package Build(Package package);
    }
}