Configuration cache
===================

During an application's startup NHibernate can take significant time
when configuring and validating it's mapping to a database. Caching the
NHibernate configuration data can reduce initial startup time by storing
the configuration to a file and avoiding the validation checks that run
when a configuration is created from scratch.

SharpArch provides interface :csharp:`INHibernateConfigurationCache` 
and :csharp:`NHibernateConfigurationFileCache` class which caches configuration
in a file under system TEMP folder.

This new feature is based on an article by Oren Eini (aka Ayende Rahien) Building a Desktop
To-Do Application with NHibernate, and a big thanks to Sandor
Drieenhuizen who provided a lot of this code.

Cache Setup
-----------

To use the configuration cache provide cache implementation to UseConfigurationCache() method 
of the NHibernateSessionFactoryBuilder class:

For example:
..code-block:: csharp

            ISessionFactory sessionFactory = new NHibernateSessionFactoryBuilder()
                .AddMappingAssemblies(new[] { HostingEnvironment.MapPath(@"~/bin/Suteki.TardisBank.Infrastructure.dll") })
                .UseAutoPersistenceModel(new AutoPersistenceModelGenerator().Generate())
                .UseConfigFile(HostingEnvironment.MapPath("~/NHibernate.config"))
                .UseConfigurationCache(new NHibernateConfigurationFileCache())
                .BuildSessionFactory();
				

Details
-------

::

    INHibernateConfigurationCache

Interface that defines two methods for loading and saving the configuration cache.

..code-block:: csharp
    NHibernate.Cfg.Configuration LoadConfiguration([NotNull] string configKey, string configPath, [NotNull] IEnumerable<string> mappingAssemblies);
    void SaveConfiguration([NotNull] string configKey, [NotNull] NHibernate.Cfg.Configuration config);


These methods are used by the NHibernateSession.AddConfiguration method to load and save a configuration object to and from a cache file. 
This interface allows others to implement their own file caching mechanism if necessary.

::

    NHibernateConfigurationFileCache

This class implements the interface and does the work of caching the configuration. 
Several methods are virtual so they can be overridden in a derived class, as may be necessary to store the cache file in a
different location or to have different logic to invalidate the cache.

The constructor takes an optional string array of file dependencies which are
used to test if the cached NHibernate configuration is current or not.
If any of the dependent file's last modified time stamp is later than
that of the cached configuration, then the cached file is discarded and
a new configuration is created from scratch. This configuration is then
serialized and saved to a file.

Note: NHibernate's XML config file and the mapping assemblies (ex: "Northwind.Data.dll") 
are automatically included when testing if the cached configuration is current.

FileCache
---------

The SharpArch.Domain project now contains a FileCache class that can
serialize and deserialize an object to a file in binary format. This
class uses a generic type parameter to define the type being serialized
and deserialized. This makes the FileCache class useful for any other
object that you might want to serialize to a file.


Configuration Serialization
---------------------------

To cache the configuration to a file, all objects contained within the
NHibernate configuration **must be serializable**. All of the default data
types included with NHibernate will serialize, but if you have any
custom data types (i.e. classes that implement IUserType), they must
also be marked with the [Serializable] attribute and, if necessary,
implement ISerializable.
