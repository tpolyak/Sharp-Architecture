namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;
    using SharpArch.PackageManagement.Contracts.Packager.Builders;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(ICreatePackageView))]
    public class CreatePackageViewModel : PropertyChangedBase, ICreatePackageView
    {
        #region Fields

        private readonly IArchiveBuilder archiveBuilder;
        private readonly IPackageBuilder packageBuilder;

        private string name;
        private string author;
        private string version;
        private string token;

        #endregion

        [ImportingConstructor]
        public CreatePackageViewModel(IPackageBuilder packageBuilder, IArchiveBuilder archiveBuilder)
        {
            this.packageBuilder = packageBuilder;
            this.archiveBuilder = archiveBuilder;
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
            var package = this.packageBuilder.Build(this.Path, new PackageMetaData { Author = this.Author, Name = this.Name, Version = this.Version });

            // this.Tokenizer.Tokenize(package);

            this.archiveBuilder.Build(package, this.Path);
        }

        public void Exit()
        {
            App.Current.Shutdown();
        }
    }
}