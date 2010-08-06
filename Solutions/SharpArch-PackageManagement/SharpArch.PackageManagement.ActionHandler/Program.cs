namespace SharpArch.PackageManagement.ActionHandler
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using SharpArch.PackageManagement.Container;
    using SharpArch.PackageManagement.Contracts.Packager;
    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Tasks;
    using SharpArch.PackageManagement.Factories;
    using SharpArch.PackageManagement.Packager;
    using SharpArch.PackageManagement.Packages;

    #endregion

    public class Program : MefContainer
    {
        [Import]
        public IPackageTask PackageTask { get; set; }

        [Import]
        public IPackageProcessor PackageProcessor { get; set; }

        [Import]
        public IPackageBuilder PackageBuilder { get; set; }

        public static void Main(string[] args)
        {
            new Program().Run(args[0]);
        }
        
        private void Run(string path)
        {
            var package = PackageFactory.Get(SharpArchitecturePackage.Location);

            package.Manifest.InstallRoot = path;

            Console.WriteLine("Processing package '{0}'", package.Manifest.Name);

            var newPackage = this.PackageBuilder.Build(path);

            //// this.PackageTask.Execute(package);

            //// this.PackageProcessor.Process(path, "MyTest.App");
        }
    }
}