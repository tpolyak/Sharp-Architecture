using System.Linq;

namespace SharpArch.PackageManagement.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;

    using SharpArch.PackageManagement.Contracts.Packager.Processors;
    using SharpArch.PackageManagement.Contracts.Packager.Tokeniser;

    #endregion

    [Export(typeof(IPackageProcessor))]
    public class PackageProcessor : IPackageProcessor
    {
        private readonly IArtefactProcessor artefactProcessor;
        private readonly ITemplateTokeniser templateTokeniser;
        private readonly ICleanUpProcessor cleanUpProcessor;

        [ImportingConstructor]
        public PackageProcessor(
            [Import("FilteredFileSystemArtefactProcessor")]IArtefactProcessor artefactProcessor,
            ICleanUpProcessor cleanUpProcessor,
            ITemplateTokeniser templateTokeniser)
        {
            this.artefactProcessor = artefactProcessor;
            this.cleanUpProcessor = cleanUpProcessor;
            this.templateTokeniser = templateTokeniser;
        }

        public void Process(string path, string name)
        {
            var files = this.artefactProcessor.RetrieveFiles(path);

            foreach (var file in files)
            {
                this.templateTokeniser.Tokenise(file, name);
            }

            var directories = this.artefactProcessor.RetrieveDirectories(path).ToList();

            foreach (var directory in directories)
            {
                if (directory.Contains("__NAME__"))
                {
                    this.cleanUpProcessor.Process(directory);
                }
            }
        }
    }
}