namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Tasks;
    using SharpArch.PackageManagement.Factories;
    using SharpArch.PackageManagement.Packages;

    #endregion

    [Export(typeof(IDeployPackageView))]
    public class DeployPackageViewModel : PropertyChangedBase, IDeployPackageView
    {
        #region Fields

        private readonly IPackageTask packageTask;
        private readonly IPackageProcessor packageProcessor;

        private string name;

        #endregion

        [ImportingConstructor]
        public DeployPackageViewModel(IPackageTask packageTask, IPackageProcessor packageProcessor)
        {
            this.packageTask = packageTask;
            this.packageProcessor = packageProcessor;
        }

        public bool CanDeployPackage
        {
            get { return !string.IsNullOrWhiteSpace(this.Name); }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.NotifyOfPropertyChange(() => this.Name);
                this.NotifyOfPropertyChange(() => this.CanDeployPackage);
            }
        }

        public string Path
        {
            get;
            set;
        }

        public void DeployPackage()
        {
            // Package Repository
            var package = PackageFactory.Get(SharpArchitecturePackage.Location);

            package.Manifest.InstallRoot = this.Path;

            this.packageTask.Execute(package);

            this.packageProcessor.Process(this.Path, "MyTest.App");
        }

        public void Exit()
        {
            App.Current.Shutdown();
        }
    }
}