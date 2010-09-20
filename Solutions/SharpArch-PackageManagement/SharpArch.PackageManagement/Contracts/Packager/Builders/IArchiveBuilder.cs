namespace SharpArch.PackageManagement.Contracts.Packager.Builders
{
    using SharpArch.PackageManagement.Domain.Packages;

    public interface IArchiveBuilder
    {
        void Build(Package package, string path);
    }
}