namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;

    #endregion

    [Export(typeof(ICreatePackageView))]
    public class CreatePackageViewModel : PropertyChangedBase, ICreatePackageView
    {
        private string name;
        private string author;
        private string version;

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
            get { return !string.IsNullOrWhiteSpace(this.Author) && !string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.Version); }
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
            MessageBox.Show(string.Format("Hello {0}!", this.Name));
        }

        public void Exit()
        {
            App.Current.Shutdown();
        }
    }
}