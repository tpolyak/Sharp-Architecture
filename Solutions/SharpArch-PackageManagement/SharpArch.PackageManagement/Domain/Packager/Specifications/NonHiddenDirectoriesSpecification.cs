namespace SharpArch.PackageManagement.Domain.Packager.Specifications
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Specifications;
    using SharpArch.PackageManagement.Framework.Specifications;

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