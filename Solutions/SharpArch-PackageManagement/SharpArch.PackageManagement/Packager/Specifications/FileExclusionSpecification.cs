namespace SharpArch.PackageManagement.Packager.Specifications
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager;
    using SharpArch.PackageManagement.Contracts.Packager.Specifications;
    using SharpArch.PackageManagement.Specifications;

    #endregion;

    [Export(typeof(IFileExclusionsSpecification))]
    public class FileExclusionSpecification : QuerySpecification<string>, IFileExclusionsSpecification
    {
        private readonly List<string> exclusions = new List<string>();

        public FileExclusionSpecification()
        {
            this.exclusions = new List<string> { ".exe", ".dll", ".pdb", ".jpg", ".png", ".gif", ".mst", ".msi", ".msm", ".gitignore", ".idx", ".pack" };
        }

        public override System.Linq.Expressions.Expression<System.Func<string, bool>> MatchingCriteria
        {
            get { return f => !this.exclusions.Contains(new FileInfo(f).Extension.ToLowerInvariant()); }
        }
    }
}