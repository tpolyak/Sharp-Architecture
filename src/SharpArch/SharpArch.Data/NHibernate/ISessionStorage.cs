using NHibernate;

namespace SharpArch.Data.NHibernate
{
    public interface ISessionStorage
    {
        ISession Session { get; set; }
        string FactoryKey { get; }
    }
}
