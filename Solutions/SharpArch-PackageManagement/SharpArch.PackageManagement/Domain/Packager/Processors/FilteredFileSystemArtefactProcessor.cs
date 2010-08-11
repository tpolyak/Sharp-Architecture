namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packager.Specifications;

    #endregion

    [Export("FilteredFileSystemArtefactProcessor", typeof(IArtefactProcessor))]
    public class FilteredFileSystemArtefactProcessor : FileSystemArtefactProcessor
    {
        private readonly IFileExclusionsSpecification fileExclusionsSpecification;

        [ImportingConstructor]
        public FilteredFileSystemArtefactProcessor(IFileExclusionsSpecification fileExclusionsSpecification)
        {
            this.fileExclusionsSpecification = fileExclusionsSpecification;
        }

        public override IEnumerable<string> RetrieveFiles(string path)
        {
            return this.fileExclusionsSpecification.SatisfyingElementsFrom(base.RetrieveFiles(path).AsQueryable());
        }
    }
}