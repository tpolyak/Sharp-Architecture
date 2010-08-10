namespace SharpArch.PackageManagement.Domain.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using ICSharpCode.SharpZipLib.Zip;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IArchiveBuilder))]
    public class ArchiveBuilder : IArchiveBuilder
    {
        public void Build(Package package, string path)
        {
            var archive = ZipFile.Create(Path.Combine(path, package.Manifest.Name) + ".zip");
            
            archive.BeginUpdate();

            foreach (var manifestFile in package.Manifest.Files)
            {
                archive.Add(Path.Combine(path, manifestFile.File), manifestFile.File);
            }

            archive.CommitUpdate();
            archive.Close();
        }
    }
}