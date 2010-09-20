namespace SharpArch.PackageManagement.Domain.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using ICSharpCode.SharpZipLib.Zip;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Infrastructure;

    #endregion

    [Export(typeof(IArchiveBuilder))]
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(Package package, string path)
        {
            var archiveName = package.Manifest.Name.ToLowerInvariant().Replace(" ", "-") + "-v" + package.Manifest.Version;
            var archive = ZipFile.Create(Path.Combine(FilePaths.PackageRepository, archiveName) + ".pkg");
            
            archive.BeginUpdate();

            foreach (var manifestFile in package.Manifest.Files)
            {
                archive.Add(Path.Combine(path, manifestFile.File), manifestFile.File.Replace(package.TemplatePath, string.Empty));
            }

            archive.CommitUpdate();
            archive.Close();
        }
    }
}