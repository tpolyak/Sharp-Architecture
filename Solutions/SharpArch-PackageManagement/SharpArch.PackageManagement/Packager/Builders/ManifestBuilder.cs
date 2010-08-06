namespace SharpArch.PackageManagement.Packager.Builders
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IManifestBuilder))]
    public class ManifestBuilder : IManifestBuilder
    {
        private readonly IArtefactProcessor artefactProcessor;

        [ImportingConstructor]
        public ManifestBuilder(IArtefactProcessor artefactProcessor)
        {
            this.artefactProcessor = artefactProcessor;
        }

        public Manifest Build(string path, IPackageMetaData packageMetaData)
        {
            var files = this.artefactProcessor.RetrieveFiles(path);

            var manifest = new Manifest
                {
                    Author = packageMetaData.Author,
                    Id = Guid.NewGuid(),
                    Name = packageMetaData.Name,
                    Version = packageMetaData.Version
                };

            foreach (var file in files)
            {
                manifest.Files.Add(new ManifestFile { File = StripParentPath(path, file) });
            }

            return manifest;
        }

        private static string StripParentPath(string parentPath, string filePath)
        {
            return filePath.Replace(string.Concat(parentPath, "\\"), string.Empty);
        }
    }
}