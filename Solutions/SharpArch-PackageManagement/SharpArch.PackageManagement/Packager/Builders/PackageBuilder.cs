namespace SharpArch.PackageManagement.Packager.Builders
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Builders;
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

        public Package Build(string path)
        {
            var package = new Package { Manifest = this.manifestBuilder.Build(path) };

            return package;
        }
    }
}