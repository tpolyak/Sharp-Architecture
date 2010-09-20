namespace SharpArch.PackageManagement.Contracts.Packager.Tokeniser
{
    public interface ITemplateTokeniser
    {
        void Tokenise(string file, string token);
    }
}