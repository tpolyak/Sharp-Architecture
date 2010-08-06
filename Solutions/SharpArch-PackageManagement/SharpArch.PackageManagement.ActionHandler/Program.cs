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
    using SharpArch.PackageManagement.Domain.Packages;
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

        [Import]
        public IArchiveBuilder ArchiveBuilder { get; set; }

        public static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                new Program().BuildPackage(args[1]);
            }
            else if (args.Length == 3)
            {
                new Program().DeployPackage(args[1], args[2]); 
            }
        }
        
        private void BuildPackage(string path)
        {
            /*
             Clone Directory
             Tokenise files contents, then file name, the directory
             Build Package
             Archive Package
            */

            var package = this.PackageBuilder.Build(path, new PackageMetaData { Author = "Howard", Name = "Sharp Architecture", Version = "1.0.0.0" });

            this.ArchiveBuilder.Build(package, path);
        }

        private void DeployPackage(string packageid, string path)
        {
            /*
             Get Package
             Set Package Install Root
             Unzip package to destication
             Tokenise files contents, then file name, the directory
            */

            var package = PackageFactory.Get(SharpArchitecturePackage.Location);
            
            package.Manifest.InstallRoot = path;

            Console.WriteLine("Processing package '{0}'", package.Manifest.Name);
            
            this.PackageTask.Execute(package);
            
            ////this.PackageProcessor.Process(path, "MyTest.App");
        }
    }
}