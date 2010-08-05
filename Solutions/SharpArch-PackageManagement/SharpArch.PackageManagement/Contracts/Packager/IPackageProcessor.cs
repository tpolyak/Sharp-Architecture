namespace SharpArch.PackageManagement.Contracts.Packager
{
    public interface IPackageProcessor
    {
        void Process(string path, string name);
    }
}