namespace SharpArch.PackageManagement.Contracts.Packages
{
    #region Using Directives

    using System;
    using System.Linq;

    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    public interface IPackageRepository
    {
        IQueryable<Package> FindAll();
        
        Package FindOne(Guid id);
    }
}