namespace SharpArch.PackageManagement.Contracts.Packager.Processors
{
    public interface IRenameFileProcessor
    {
        void Rename(string oldName, string newName);
    }
}