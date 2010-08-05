namespace SharpArch.PackageManagement.Contracts.Tasks
{
    using SharpArch.PackageManagement.Domain.Packages;

    public interface IPackageTask
    {
        void Execute(Package package);
    }
}
