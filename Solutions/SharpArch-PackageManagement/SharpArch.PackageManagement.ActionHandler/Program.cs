namespace SharpArch.PackageManagement.ActionHandler
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    
    using SharpArch.PackageManagement.Contracts;
    using SharpArch.PackageManagement.Factories;
    using SharpArch.PackageManagement.Packages;

    #endregion

    public class Program
    {
        [Import]
        public IPackageTask PackageTask { get; set; }

        public static void Main(string[] args)
        {
            new Program().Run(args[0]);
        }

        public void Run(string path)
        {
            this.Compose();

            var package = PackageFactory.Create(SharpArchitecturePackage.Location);

            package.Manifest.InstallRoot = path;

            Console.WriteLine("Processing package '{0}'", package.Manifest.Name);

            this.PackageTask.Execute(package);
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