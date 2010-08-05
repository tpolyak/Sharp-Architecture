namespace SharpArch.PackageManagement.Contracts.Packager
{
    using System.Collections.Generic;

    public interface IArtefactProcessor
    {
        IEnumerable<string> RetrieveFiles(string path);

        IEnumerable<string> RetrieveDirectories(string path);
    }
}