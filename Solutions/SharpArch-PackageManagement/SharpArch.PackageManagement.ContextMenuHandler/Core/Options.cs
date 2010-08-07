namespace SharpArch.PackageManagement.ContextMenuHandler.Core
{
    using CommandLine;

    public class Options
    {
        [Option("c", "Create Package")]
        public string CreatePackagePath = null;

        [Option("d", "Deploy Package")]
        public string DeployPackagePath = null;
    }
}