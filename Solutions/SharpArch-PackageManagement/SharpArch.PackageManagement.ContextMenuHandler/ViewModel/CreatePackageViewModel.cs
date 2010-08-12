namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;
    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packager.Tokeniser;
    using SharpArch.PackageManagement.Domain.Factories;
    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Framework.Threading;
    using SharpArch.PackageManagement.Infrastructure;

    #endregion

    [Export(typeof(ICreatePackageView))]
    public class CreatePackageViewModel : PropertyChangedBase, ICreatePackageView
    {
        #region Fields

        private readonly IArchiveBuilder archiveBuilder;
        private readonly ICleanUpProcessor cleanUpProcessor;
        private readonly IClonePackageBuilder clonePackageBuilder;
        private readonly IPackageBuilder packageBuilder;
        private readonly IPackageTokeniser packageTokeniser;

        private string name;
        private string author;
        private string version;
        private string token;

        #endregion

        [ImportingConstructor]
        public CreatePackageViewModel(
            IArchiveBuilder archiveBuilder, 
            ICleanUpProcessor cleanUpProcessor, 
            IClonePackageBuilder clonePackageBuilder,
            IPackageBuilder packageBuilder,
            IPackageTokeniser packageTokeniser)
        {
            this.archiveBuilder = archiveBuilder;
            this.cleanUpProcessor = cleanUpProcessor;
            this.clonePackageBuilder = clonePackageBuilder;
            this.packageBuilder = packageBuilder;
            this.packageTokeniser = packageTokeniser;
        }

        public string Author
        {
            get
            {
                return this.author;
            }

            set
            {
                this.author = value;
                this.NotifyOfPropertyChange(() => this.Author);
                this.NotifyOfPropertyChange(() => this.CanCreatePackage);
            }
        }

        public bool CanCreatePackage
        {
            get 
            { 
                return !string.IsNullOrWhiteSpace(this.Author) && 
                       !string.IsNullOrWhiteSpace(this.Name) && 
                       !string.IsNullOrWhiteSpace(this.Token) && 
                       !string.IsNullOrWhiteSpace(this.Version); 
            }
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
                this.NotifyOfPropertyChange(() => this.CanCreatePackage);
            }
        }

        public string Path
        {
            get;
            set;
        }

        public string Token
        {
            get
            {
                return this.token;
            }

            set
            {
                this.token = value;
                this.NotifyOfPropertyChange(() => this.Token);
                this.NotifyOfPropertyChange(() => this.CanCreatePackage);
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }

            set
            {
                this.version = value;
                this.NotifyOfPropertyChange(() => this.Version);
                this.NotifyOfPropertyChange(() => this.CanCreatePackage);
            }
        }

        public void CreatePackage()
        {
            BackgroundWorkerManager.RunBackgroundWork(this.ExecuteCreatePackage, this.ExecuteCreatePackageComplete);
        }

        private void ExecuteCreatePackage()
        {
            var package = this.packageBuilder.Build(this.Path, new PackageMetaData { Author = this.Author, Name = this.Name, Version = this.Version });

            var clonedPackage = this.clonePackageBuilder.Build(package);
            var tokenisedPackage = this.packageTokeniser.Tokenise(clonedPackage, this.Token);

            this.archiveBuilder.Build(tokenisedPackage, this.Path);
            this.cleanUpProcessor.Process(FilePaths.TemporaryPackageRepository);
        }

        private void ExecuteCreatePackageComplete(RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
               MessageBox.Show("Package Created and Deployed to the Package Repository");
            }
            else
            {
                // Do some error handling
            }
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}