namespace SharpArch.PackageManagement.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(IPackageProcessor))]
    public class PackageProcessor : IPackageProcessor
    {
        [Import("Selective")]
        private IArtefactProcessor ArtefactProcessor { get; set; }

        public void Process(string path, string name)
        {
            var files = this.ArtefactProcessor.RetrieveFiles(path);
            var directories = this.ArtefactProcessor.RetrieveFiles(path);
        }
    }
}