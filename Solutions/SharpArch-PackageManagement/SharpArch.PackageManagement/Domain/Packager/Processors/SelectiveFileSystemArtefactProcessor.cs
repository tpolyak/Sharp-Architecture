namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packager.Specifications;

    #endregion

    [Export("Selective", typeof(IArtefactProcessor))]
    public class SelectiveFileSystemArtefactProcessor : FileSystemArtefactProcessor
    {
        private readonly IDirectoryExclusionsSpecification directoryExclusionsSpecification;
        private readonly IFileExclusionsSpecification fileExclusionsSpecification;

        [ImportingConstructor]
        public SelectiveFileSystemArtefactProcessor(IDirectoryExclusionsSpecification directoryExclusionsSpecification, IFileExclusionsSpecification fileExclusionsSpecification)
        {
            this.directoryExclusionsSpecification = directoryExclusionsSpecification;
            this.fileExclusionsSpecification = fileExclusionsSpecification;
        }

        public override IEnumerable<string> RetrieveDirectories(string path)
        {
            return this.directoryExclusionsSpecification.SatisfyingElementsFrom(base.RetrieveDirectories(path).AsQueryable());
        }

        public override IEnumerable<string> RetrieveFiles(string path)
        {
            return this.fileExclusionsSpecification.SatisfyingElementsFrom(base.RetrieveFiles(path).AsQueryable());
        }
    }
}