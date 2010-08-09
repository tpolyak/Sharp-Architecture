namespace SharpArch.PackageManagement.Contracts.Tasks
{
    using System;

    using SharpArch.PackageManagement.Domain.Packages;

    public interface IPackageTask
    {
        event EventHandler<PackageProgressEventArgs> Progress;

        void Execute(Package package);
    }
}
