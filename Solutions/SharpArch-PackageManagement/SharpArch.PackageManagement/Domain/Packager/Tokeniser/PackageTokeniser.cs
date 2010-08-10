namespace SharpArch.PackageManagement.Domain.Packager.Tokeniser
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;
    using System.Text.RegularExpressions;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packager.Tokeniser;
    using SharpArch.PackageManagement.Domain.Packages;

    #endregion

    [Export(typeof(IPackageTokeniser))]
    public class PackageTokeniser : IPackageTokeniser
    {
        private readonly IRenameFileProcessor renameFileProcessor;

        [ImportingConstructor]
        public PackageTokeniser(IRenameFileProcessor renameFileProcessor)
        {
            this.renameFileProcessor = renameFileProcessor;
        }

        public Package Tokenise(Package package, string token)
        {
            string tokenisedName;

            foreach (var manifestFile in package.Manifest.Files)
            {
                tokenisedName = Tokenise(token, manifestFile.File);
                this.renameFileProcessor.Rename(manifestFile.File, tokenisedName);
                manifestFile.File = tokenisedName;
            }

            return package;
        }

        private static string Tokenise(string token, string value)
        {
            return Regex.Replace(
                value,
                token,
                delegate { return "__NAME__"; });
        }
    }
}
