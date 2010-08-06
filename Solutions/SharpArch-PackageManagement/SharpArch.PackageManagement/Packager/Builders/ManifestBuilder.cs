namespace SharpArch.PackageManagement.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IManifestBuilder))]
    public class ManifestBuilder : IManifestBuilder
    {
        [Import]
        private IArtefactProcessor ArtefactProcessor { get; set; }

        public Manifest Build(string path)
        {
            var files = this.ArtefactProcessor.RetrieveFiles(path);

            var manifest = new Manifest();

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