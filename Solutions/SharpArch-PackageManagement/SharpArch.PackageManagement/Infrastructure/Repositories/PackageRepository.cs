namespace SharpArch.PackageManagement.Infrastructure.Repositories
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Domain.Factories;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IPackageRepository))]
    public class PackageRepository : IPackageRepository
    {
        private readonly string repositoryPath;
        private readonly IArtefactProcessor fileSystemArtecactProcessor;

        [ImportingConstructor]
        public PackageRepository(IArtefactProcessor fileSystemArtecactProcessor)
        {
            this.fileSystemArtecactProcessor = fileSystemArtecactProcessor;

            this.repositoryPath = FilePaths.PackageRepository;
        }

        public IQueryable<Package> FindAll()
        {
            var files = this.fileSystemArtecactProcessor.RetrieveFiles(this.repositoryPath, "*.pkg");

            return Queryable.AsQueryable<Package>(files.Select(PackageFactory.Get)
                            .Where(p => !string.IsNullOrEmpty(p.Manifest.Name)));
        }

        public Package FindOne(Guid id)
        {
            return this.FindAll().Where(p => p.Manifest.Id == id).FirstOrDefault();
        }
    }
}