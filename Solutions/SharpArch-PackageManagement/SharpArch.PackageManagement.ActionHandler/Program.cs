namespace SharpArch.PackageManagement.ActionHandler
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager;
    using SharpArch.PackageManagement.Contracts.Tasks;
    using SharpArch.PackageManagement.Factories;
    using SharpArch.PackageManagement.Packager;
    using SharpArch.PackageManagement.Packages;

    #endregion

    public class Program
    {
        [Import]
        public IPackageTask PackageTask { get; set; }

        [Import]
        public IPackageProcessor PackageProcessor { get; set; }

        public static void Main(string[] args)
        {
            new Program().Run(args[0]);
        }

        private void Run(string path)
        {
            this.Compose();

            var package = PackageFactory.Create(SharpArchitecturePackage.Location);

            package.Manifest.InstallRoot = path;

            Console.WriteLine("Processing package '{0}'", package.Manifest.Name);

            //// this.PackageTask.Execute(package);
            
            this.PackageProcessor.Process(path, "MyTest.App");
        }

        private void Compose()
        {
            var catalog = new AggregateCatalog();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            catalog.Catalogs.Add(new DirectoryCatalog(baseDirectory));
            var container = new CompositionContainer(catalog);
         
            container.ComposeParts(this);
        }
    }
}