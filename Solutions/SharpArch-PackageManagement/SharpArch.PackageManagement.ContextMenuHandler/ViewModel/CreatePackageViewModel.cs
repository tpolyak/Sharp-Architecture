namespace SharpArch.PackageManagement.ContextMenuHandler.ViewModel
{
    #region Using Directives

    using System;
    using System.ComponentModel.Composition;

    using Caliburn.Micro;

    using SharpArch.PackageManagement.ContextMenuHandler.Contracts;

    #endregion

    [Export(typeof(ICreatePackageView))]
    public class CreatePackageViewModel : PropertyChangedBase, ICreatePackageView
    {
        public string Path
        {
            get;
            set;
        }
    }
}