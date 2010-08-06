namespace SharpArch.PackageManagement.Contracts.Packager.Builders
{
    using SharpArch.PackageManagement.Domain.Packages;

    public interface IManifestBuilder
    {
        Manifest Build(string path);
    }
}