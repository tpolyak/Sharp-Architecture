namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(IRenameFileProcessor))]
    public class RenameFileProcessor : IRenameFileProcessor
    {
        public void Rename(string oldName, string newName)
        {
            File.Move(oldName, newName);
        }
    }
}