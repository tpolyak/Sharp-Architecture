namespace SharpArch.PackageManagement.ContextMenuHandler.Domain
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    public class PackageCollection : ObservableCollection<Package>
    {
        public PackageCollection(IEnumerable<Package> collection) : base(collection)
        {
        }

        /// <summary>
        /// Queue a request to clear this collection
        /// </summary>
        public bool RequestClear { get; set; }
    }
}