namespace SharpArch.Data.NHibernate
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using global::NHibernate.Cfg;

    /// <summary>
    /// File cache implementation of INHibernateConfigurationCache.  Saves and loads a
    /// seralized version of <see cref="Configuration"/> to a temporary file location.
    /// </summary>
    /// <remarks>Seralizing a <see cref="Configuration"/> object requires that all components
    /// that make up the Configuration object be Serializable.  This includes any custom NHibernate 
    /// user types implementing <see cref="NHibernate.UserTypes.IUserType"/>.</remarks>
    public class NHibernateConfigurationFileCache : INHibernateConfigurationCache
    {
        #region INHibernateConfigurationCache Members

        /// <summary>
        /// Load the <see cref="Configuration"/> object from a cache.
        /// </summary>
        /// <param name="configKey">Key value to provide a unique name to the cached <see cref="Configuration"/>.</param>
        /// <param name="configFile">NHibernate configuration xml file.  This is used to determine if the 
        /// cached <see cref="Configuration"/> is out of date or not.</param>
        /// <returns>If an up to date cached object is available, a <see cref="Configuration"/> 
        /// object, otherwise null.</returns>
        public Configuration LoadConfiguration(string configKey, string configFile, string[] mappingAssemblies) {
            if (!string.IsNullOrEmpty(configKey) && !IsSeralizedConfigCurrent(configKey, configFile, mappingAssemblies)) {
                return null;
            }

            return Deseralize(SeralizedConfigFileName(configKey));
        }

        /// <summary>
        /// Save the <see cref="Configuration"/> object to cache to a temporary file.
        /// </summary>
        /// <param name="configKey">Key value to provide a unique name to the cached <see cref="Configuration"/>.</param>
        /// <param name="config">Configuration object to save.</param>
        public void SaveConfiguration(string configKey, Configuration config) {
            Serialize(config, SeralizedConfigFileName(configKey));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Deserializes a data file from into a <see cref="Configuration"/> object.
        /// </summary>
        /// <param name="fileName">Full path to file containing seralized data.</param>
        /// <returns>Configuration object deseralized from data file.</returns>
        protected virtual Configuration Deseralize(string fileName) {
            Configuration config;
            try {
                using (FileStream file = File.Open(fileName, FileMode.Open)) {
                    BinaryFormatter bf = new BinaryFormatter();
                    config = bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception) {
                return null;
            }

            return config;
        }

        /// <summary>
        /// Serialize the given Configuration object and store at the given 
        /// file name.
        /// </summary>
        /// <param name="config">Configuration object to serialize.</param>
        /// <param name="fileName">Full path file name where to store the serialized data.</param>
        protected virtual void Serialize(Configuration config, string fileName) {
            using (FileStream file = File.Open(fileName, FileMode.Create)) {
                new BinaryFormatter().Serialize(file, config);
            }
        }

        /// <summary>
        /// Determines if the seralized configuration cache file is up to date relative 
        /// to the NHibernate xml configuration file.
        /// </summary>
        /// <param name="cacheFile">Name of seralized configuration cache file.</param>
        /// <param name="configFile">File name of NHibernate configuration xml file.</param>
        /// <param name="mappingAssemblies">Array of file names for assemblies that provide 
        /// data mapping information for the NHibernate configuration.  If the date stamp
        /// on any of these mapping files is more recent than the configuration file cache
        /// then it is out of date.</param>
        /// <returns>True if the cached configuration is up to date, otherwise false.</returns>
        protected virtual bool IsSeralizedConfigCurrent(string cacheFile, string configFile, string[] mappingAssemblies) {
            if (string.IsNullOrEmpty(configFile) || string.IsNullOrEmpty(cacheFile)) {
                return false;
            }

            Assembly assembly = Assembly.GetCallingAssembly();
            if ((assembly == null) || (string.IsNullOrEmpty(assembly.Location))) {
                return false;
            }

            FileInfo serializedConfigInfo = new FileInfo(SeralizedConfigFileName(cacheFile));
            if (!((serializedConfigInfo != null) && serializedConfigInfo.Exists)) {
                return false;
            }

            FileInfo configFileInfo = new FileInfo(configFile);
            if (!((configFileInfo != null) && configFileInfo.Exists)) {
                return false;
            }
            if (serializedConfigInfo.LastWriteTime < configFileInfo.LastWriteTime) {
                return false;
            }

            FileInfo assemblyInfo = new FileInfo(assembly.Location);
            if ((assemblyInfo != null) && (serializedConfigInfo.LastWriteTime < assemblyInfo.LastWriteTime)) {
                return false;
            }

            foreach (string mappingAssembly in mappingAssemblies) {
                FileInfo mappingInfo = new FileInfo(mappingAssembly);
                if ((mappingInfo != null) && (mappingInfo.Exists)
                    && (serializedConfigInfo.LastWriteTime < mappingInfo.LastWriteTime)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Provide a unique temp file path/name incorporating a hash value based on 
        /// the location of the calling assembly.
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns>File path/name</returns>
        /// <remarks>The hash value is intended to avoid the file from conflicting
        /// with other applications also using this cache feature.</remarks>
        protected virtual string SeralizedConfigFileName(string configKey) {
            var fileName = string.Format("{0}-{1}.bin", configKey, Assembly.GetCallingAssembly().CodeBase.GetHashCode());
            return Path.Combine(Path.GetTempPath(), fileName);
        }

        #endregion
    }
}
