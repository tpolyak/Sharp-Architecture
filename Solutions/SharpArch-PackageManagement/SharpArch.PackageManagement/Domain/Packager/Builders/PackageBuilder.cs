namespace SharpArch.PackageManagement.Domain.Packager.Builders
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IPackageBuilder))]
    public class PackageBuilder : IPackageBuilder
    {
        private readonly IManifestBuilder manifestBuilder;

        [ImportingConstructor]
        public PackageBuilder(IManifestBuilder manifestBuilder)
        {
            this.manifestBuilder = manifestBuilder;
        }

        public Package Build(string path, IPackageMetaData packageMetaData)
        {
            return new Package { Manifest = this.manifestBuilder.Build(path, packageMetaData) };
        }
    }
}