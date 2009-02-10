using FluentNHibernate.AutoMap;

namespace SharpArch.Data.NHibernate.FluentNHibernate
{
    /// <summary>
    /// Used by <see cref="AutoPersistenceModelExtensions" /> to add auto mapping overrides 
    /// to <see cref="AutoPersistenceModel" />
    /// </summary>
    public interface IAutoPeristenceModelConventionOverride
    {
        AutoPersistenceModel Override(AutoPersistenceModel model);
    }
}
