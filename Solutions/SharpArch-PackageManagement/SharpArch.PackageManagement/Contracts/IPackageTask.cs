namespace SharpArch.PackageManagement.Contracts
{
    using SharpArch.PackageManagement.Domain;

    public interface IPackageTask
    {
        void Execute(Package package);
    }
}
