namespace SharpArch.Infrastructure.NHibernate.FluentNHibernate
{
    using System;

    using global::FluentNHibernate.Automapping;

    [CLSCompliant(false)]
    public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }
}