namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;

    #endregion

    [Export(typeof(IDeployPackageView))]
    public class DeployPackageViewModel : PropertyChangedBase, IDeployPackageView
    {
        private string name;

        public string Path
        {
            get;
            set;
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

        public void DeployPackage()
        {
            MessageBox.Show(string.Format("Deploy {0}!", this.Name));
        }

        public void Exit()
        {
            App.Current.Shutdown();
        }
    }
}