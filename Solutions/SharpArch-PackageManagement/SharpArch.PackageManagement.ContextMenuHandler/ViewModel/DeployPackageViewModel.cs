namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;
    using SharpArch.PackageManagement.ContextMenuHandler.Domain;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packages;
    using SharpArch.PackageManagement.Contracts.Tasks;
    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Factories;

    using Action = System.Action;

    #endregion

    [Export(typeof(IDeployPackageView))]
    public class DeployPackageViewModel : PropertyChangedBase, IDeployPackageView
    {
        #region Fields

        private readonly IPackageTask packageTask;
        private readonly IPackageProcessor packageProcessor;
        private readonly IPackageRepository packageRepository;

        private PackageCollection packages;
        private string name;

        #endregion

        [ImportingConstructor]
        public DeployPackageViewModel(IPackageTask packageTask, IPackageProcessor packageProcessor, IPackageRepository packageRepository)
        {
            this.packageTask = packageTask;
            this.packageProcessor = packageProcessor;
            this.packageRepository = packageRepository;

            Dispatcher = Application.Current.Dispatcher;
        }

        #region Properties

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

        public Package SelectedPackage { get; set; }

        public PackageCollection Packages
        {
            get
            {
                if (this.packages == null)
                {
                    this.Initialise();
                }

                return this.packages;
            }

            set
            {
                this.packages = value;
                this.NotifyOfPropertyChange(() => this.Packages);
                this.NotifyOfPropertyChange(() => this.CanDeployPackage);
            }
        }

        public string Path
        {
            get;
            set;
        }

        private static Dispatcher Dispatcher
        {
            get; set;
        }

        #endregion

        public void DeployPackage()
        {
            var package = this.SelectedPackage;

            package.Manifest.InstallRoot = this.Path;

            this.packageTask.Execute(package);

            this.packageProcessor.Process(this.Path, this.Name);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Initialise()
        {
            Action workAction = delegate
                {
                    var worker = new BackgroundWorker();
                    worker.DoWork += delegate
                        {
                            this.RetrievePackages();
                        };
                    worker.RunWorkerAsync();
                };

            Dispatcher.BeginInvoke(DispatcherPriority.Background, workAction);
        }

        private void RetrievePackages()
        {
            this.Packages = new PackageCollection(this.packageRepository.FindAll());
        }
    }
}