namespace SharpArch.NHibernate.FluentNHibernate
{
    using global::FluentNHibernate.Automapping;

    /// <summary>
    /// Fluent NHibernate auto-mapping model generator.
    /// </summary>
    /// <remarks>
    /// Interface implementors will be automatically executed by TestDatabaseInitializer during test database initialization.
    /// </remarks>
    public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }
}
