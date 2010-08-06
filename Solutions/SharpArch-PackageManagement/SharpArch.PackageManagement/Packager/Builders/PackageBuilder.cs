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
        [Import]
        private IManifestBuilder ManifestBuilder { get; set; }

        public Package Build(string path)
        {
            var package = new Package { Manifest = this.ManifestBuilder.Build(path) };

            return package;
        }
    }
}