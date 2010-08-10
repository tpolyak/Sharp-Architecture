namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(ICloneFileProcessor))]
    public class CloneFileProcessor : ICloneFileProcessor
    {
        public void Process(string fromPath, string toPath)
        {
            var file = new FileInfo(toPath);

            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.Copy(fromPath, toPath);
        }
    }
}