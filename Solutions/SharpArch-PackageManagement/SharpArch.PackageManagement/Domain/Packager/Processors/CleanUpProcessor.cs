namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(ICleanUpProcessor))]
    public class CleanUpProcessor : ICleanUpProcessor
    {
        public void Process(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}