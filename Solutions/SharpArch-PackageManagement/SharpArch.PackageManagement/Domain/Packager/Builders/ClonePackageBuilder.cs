namespace SharpArch.PackageManagement.Domain.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Infrastructure;

    #endregion

    [Export(typeof(IClonePackageBuilder))]
    public class ClonePackageBuilder : IClonePackageBuilder
    {
        private readonly ICloneFileProcessor cloneFileProcessor;
        private readonly string temporaryRepositoryPath;

        [ImportingConstructor]
        public ClonePackageBuilder(ICloneFileProcessor cloneFileProcessor)
        {
            this.cloneFileProcessor = cloneFileProcessor;
            this.temporaryRepositoryPath = FilePaths.TemporaryPackageRepository;
        }

        public Package Build(Package package)
        {
            foreach (var file in package.Manifest.Files)
            {
                var clonedPath = Path.Combine(this.temporaryRepositoryPath, file.File);

                this.cloneFileProcessor.Process(Path.Combine(package.Manifest.Path, file.File), clonedPath);

                file.File = clonedPath;
            }

            return package;
        }
    }
}