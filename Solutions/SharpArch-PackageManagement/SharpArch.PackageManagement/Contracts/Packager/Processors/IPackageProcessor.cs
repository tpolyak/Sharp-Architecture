namespace SharpArch.PackageManagement.Contracts.Packager.Processors
{
    public interface IPackageProcessor
    {
        void Process(string path, string name);
    }
}