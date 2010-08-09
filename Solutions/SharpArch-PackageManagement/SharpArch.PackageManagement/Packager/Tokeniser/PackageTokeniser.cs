namespace SharpArch.PackageManagement.Packager.Tokeniser
{
    #region Using Directives

    using System.ComponentModel.Composition;

    using SharpArch.PackageManagement.Contracts.Packager.Tokeniser;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IPackageTokeniser))]
    public class PackageTokeniser : IPackageTokeniser
    {
        public Package Tokenise(Package package, string token)
        {
            // Clone directory
            // 1. Tokenise Directory Names
            // 2. Tokenise File names
            // 3. Tokenise file contents

            return new Package();
        }
    }
}