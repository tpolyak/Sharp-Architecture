namespace SharpArch.PackageManagement.Repositories
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Factories;

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

            this.repositoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                @"Sharp-Architecture\Package-Repository\");
        }

        public IQueryable<Package> FindAll()
        {
            var files = this.fileSystemArtecactProcessor.RetrieveFiles(this.repositoryPath, "*.pkg");

            return files.Select(PackageFactory.Get)
                        .Where(p => !string.IsNullOrEmpty(p.Manifest.Name)).AsQueryable();
        }

        public Package FindOne(Guid id)
        {
            return this.FindAll().Where(p => p.Manifest.Id == id).FirstOrDefault();
        }
    }
}