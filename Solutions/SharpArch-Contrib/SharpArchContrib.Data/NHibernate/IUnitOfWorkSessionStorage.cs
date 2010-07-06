using SharpArch.Data.NHibernate;

namespace SharpArchContrib.Data.NHibernate {
    public interface IUnitOfWorkSessionStorage : ISessionStorage {
        void EndUnitOfWork(bool closeSessions);
    }
}