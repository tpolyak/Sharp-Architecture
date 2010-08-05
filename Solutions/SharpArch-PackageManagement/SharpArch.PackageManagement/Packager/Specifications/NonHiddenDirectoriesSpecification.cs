namespace SharpArch.PackageManagement.Packager.Specifications
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager;
    using SharpArch.PackageManagement.Specifications;

    #endregion

    [Export(typeof(IDirectoryExclusionsSpecification))]
    public class NonHiddenDirectoriesSpecification : QuerySpecification<string>, IDirectoryExclusionsSpecification
    {
        public override System.Linq.Expressions.Expression<System.Func<string, bool>> MatchingCriteria
        {
            get { return d => (new DirectoryInfo(d).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden; }
        }
    }
}