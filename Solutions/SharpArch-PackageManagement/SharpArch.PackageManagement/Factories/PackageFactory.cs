namespace SharpArch.PackageManagement.Factories
{
    #region Using Directives

    using System.Xml.Serialization;

    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    public static class PackageFactory
    {
        public static Package Get(string path)
        {
            var package = new ICSharpCode.SharpZipLib.Zip.ZipFile(path);
            var manifestFile = package.GetEntry("manifest.xml");

            var manifestXmlStream = package.GetInputStream(manifestFile.ZipFileIndex);

            var serializer = new XmlSerializer(typeof(Manifest));
            var manifest = (Manifest)serializer.Deserialize(manifestXmlStream);
            
            manifest.Path = path;

            return new Package { Manifest = manifest };
        }
    }
}