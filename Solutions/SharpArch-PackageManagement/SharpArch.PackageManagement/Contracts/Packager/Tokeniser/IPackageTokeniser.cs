namespace SharpArch.PackageManagement.Contracts.Packager.Tokeniser
{
    #region Using Directives

    using SharpArch.PackageManagement.Domain.Packages; 

    #endregion;

    public interface IPackageTokeniser
    {
        Package Tokenise(Package package, string token);
    }
}