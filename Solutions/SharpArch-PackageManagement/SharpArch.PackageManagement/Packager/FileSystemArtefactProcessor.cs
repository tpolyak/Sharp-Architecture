namespace SharpArch.PackageManagement.Packager
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using SharpArch.PackageManagement.Contracts.Packager;

    #endregion

    [Export(typeof(IArtefactProcessor))]
    public class FileSystemArtefactProcessor : IArtefactProcessor
    {
        [Import(typeof(IDirectoryExclusionsSpecification))]
        public IDirectoryExclusionsSpecification DirectoryExclusionsSpecification { get; set; }

        [Import(typeof(IFileExclusionsSpecification))]
        public IFileExclusionsSpecification FileExclusionsSpecification { get; set; }

        public IEnumerable<string> RetrieveFiles(string path)
        {
            return this.FileExclusionsSpecification.SatisfyingElementsFrom(Flatten(path, Directory.GetDirectories)
                                                   .SelectMany(dir => Directory.EnumerateFiles(dir, "*.*"))
                                                   .AsQueryable());
        }

        public IEnumerable<string> RetrieveDirectories(string path)
        {
            return this.DirectoryExclusionsSpecification.SatisfyingElementsFrom(Flatten(path, Directory.EnumerateDirectories)
                                                        .AsQueryable());
        }

        private static IEnumerable<T> Flatten<T>(T item, Func<T, IEnumerable<T>> next)
        {
            yield return item;

            foreach (T flattenedChild in next(item).SelectMany<T, T>(child => Flatten(child, next)))
            {
                yield return flattenedChild;
            }
        }
    }
}