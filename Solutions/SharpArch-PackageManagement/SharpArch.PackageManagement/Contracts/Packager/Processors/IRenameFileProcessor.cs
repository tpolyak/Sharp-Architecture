namespace SharpArch.PackageManagement.Contracts.Packager.Processors
{
    public interface IRenameFileProcessor
    {
        void Process(string oldName, string newName);
    }
}