using FluentNHibernate.AutoMap;

namespace SharpArch.Data.NHibernate.FluentNHibernate
{
    public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }
}
