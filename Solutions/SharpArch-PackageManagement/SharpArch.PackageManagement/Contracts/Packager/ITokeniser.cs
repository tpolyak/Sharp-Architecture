namespace SharpArch.PackageManagement.Contracts.Packager
{
    public interface ITokeniser
    {
        void Replace(string oldToken, string newToken);
    }
}