namespace SharpArch.PackageManagement.Domain.Packages
{
    public class Package
    {
        public Package()
        {
            this.Manifest = new Manifest();
        }

        public Manifest Manifest { get; set; }
    }
}