namespace SharpArch.PackageManagement.Domain.Factories
{
    #region Using Directives

    using SharpArch.PackageManagement.Domain.Packages;
    using SharpArch.PackageManagement.Framework.Serialization;

    #endregion

    public static class ManifestFactory
    {
        public static void Save(string path, Manifest manifest)
        {
            Serializer.SaveInstance(manifest, path);
        }
    }
}