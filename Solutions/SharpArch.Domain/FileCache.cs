namespace SharpArch.Domain
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    ///     Provides file cache helper methods.
    /// </summary>
    public static class FileCache
    {
        /// <summary>
        ///     Deserializes a data file into an object of type {T}.
        /// </summary>
        /// <typeparam name="T">Type of object to deseralize and return.</typeparam>
        /// <param name="path">Full path to file containing seralized data.</param>
        /// <returns>If object is successfully deseralized, the object of type {T}, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the path parameter is null or empty.</exception>
        public static T RetrieveFromCache<T>(string path) where T : class
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            try
            {
                using (var file = File.Open(path, FileMode.Open))
                {
                    return Load<T>(file);
                }
            }
            catch
            {
                // Return null if the object can't be deseralized
                return null;
            }
        }

        /// <summary>
        ///     Serializes the given object of type {T} to a file at the given path.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize and store in a file.</param>
        /// <param name="path">Full path of file to store the serialized data.</param>
        /// <exception cref="ArgumentNullException">Thrown if obj or path parameters are null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path" /> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />. </exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
        /// <exception cref="NotSupportedException"><paramref name="path" /> is in an invalid format. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive). </exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file. </exception>
        /// <exception cref="UnauthorizedAccessException"><paramref name="path" /> specified a file that is read-only.
        /// -or- This operation is not supported on the current platform.
        /// -or- <paramref name="path" /> specified a directory.
        /// -or- The caller does not have the required permission. 
        /// -or specified file is a hidden file.</exception>
        public static void StoreInCache<T>(T obj, string path) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            using (var file = File.Open(path, FileMode.Create))
            {
                Save(file, obj);
            }
        }

        internal static void Save<T>(Stream file, T obj) where T : class
        {
            new BinaryFormatter().Serialize(file, obj);
        }

        internal static T Load<T>(Stream file) where T : class
        {
            return new BinaryFormatter().Deserialize(file) as T;
        }


    }
}