namespace SharpArch.PackageManagement.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IClonePackageBuilder))]
    public class ClonePackageBuilder : IClonePackageBuilder
    {
        public Package Build(Package package)
        {
            return new Package();
        }
    }
}