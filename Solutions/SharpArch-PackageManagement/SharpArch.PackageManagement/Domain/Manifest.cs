namespace SharpArch.PackageManagement.Domain
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    #endregion

    [XmlRoot("Manifest")]
    public class Manifest
    {
        public string Author { get; set; }

        [XmlArray]
        [XmlArrayItem("File")]
        public List<ManifestFile> Files { get; set; }

        public Guid Id { get; set; }

        public string InstallRoot { get; set; }

        public string Name { get; set; }

        [XmlIgnore]
        public string Path { get; set; }

        public string Version { get; set; }
    }
}