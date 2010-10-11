namespace SharpArch.Data.NHibernate.FluentNHibernate
{
    using System;

    using global::FluentNHibernate.Automapping;

    [CLSCompliant(false)]
    public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }
}