namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(IPackageProcessor))]
    public class PackageProcessor : IPackageProcessor
    {
        private readonly IArtefactProcessor artefactProcessor;

        [ImportingConstructor]
        public PackageProcessor([Import("FilteredFileSystemArtefactProcessor")]IArtefactProcessor artefactProcessor)
        {
            this.artefactProcessor = artefactProcessor;
        }

        public void Process(string path, string name)
        {
            var files = this.artefactProcessor.RetrieveFiles(path);
            var directories = this.artefactProcessor.RetrieveFiles(path);
        }
    }
}