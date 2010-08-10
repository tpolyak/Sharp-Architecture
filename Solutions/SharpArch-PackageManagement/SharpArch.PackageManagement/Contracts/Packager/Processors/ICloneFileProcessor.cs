namespace SharpArch.PackageManagement.Contracts.Packager.Processors
{
    public interface ICloneFileProcessor
    {
        void Process(string fromPath, string toPath);
    }
}