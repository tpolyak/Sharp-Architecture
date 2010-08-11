namespace SharpArch.PackageManagement.Domain.Packages
{
    public class Package
    {
        public Package()
        {
            this.Manifest = new Manifest();
        }

        public string ClonedPath { get; set; }

        public string TemplatePath { get; set; }

        public Manifest Manifest { get; set; }
    }
}