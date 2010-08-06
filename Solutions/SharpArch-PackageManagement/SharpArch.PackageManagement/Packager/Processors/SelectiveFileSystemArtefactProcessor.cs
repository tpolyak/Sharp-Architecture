namespace SharpArch.PackageManagement.Packager.Processors
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
        [Import(typeof(IDirectoryExclusionsSpecification))]
        private IDirectoryExclusionsSpecification DirectoryExclusionsSpecification { get; set; }

        [Import(typeof(IFileExclusionsSpecification))]
        private IFileExclusionsSpecification FileExclusionsSpecification { get; set; }

        public override IEnumerable<string> RetrieveDirectories(string path)
        {
            return this.DirectoryExclusionsSpecification.SatisfyingElementsFrom(base.RetrieveDirectories(path).AsQueryable());
        }

        public override IEnumerable<string> RetrieveFiles(string path)
        {
            return this.FileExclusionsSpecification.SatisfyingElementsFrom(base.RetrieveFiles(path).AsQueryable());
        }
    }
}