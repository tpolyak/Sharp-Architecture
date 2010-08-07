namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;

    #endregion

    [Export(typeof(IDeployPackageView))]
    public class DeployPackageViewModel : PropertyChangedBase, IDeployPackageView
    {
        public string Path
        {
            get;
            set;
        }
    }
}