namespace SharpArch.PackageManagement.Domain
{
    using System.Xml.Serialization;

    public class ManifestFile
    {
        [XmlText]
        public string File { get; set; }

        [XmlAttribute]
        public string InstallPath { get; set; }
    }
}