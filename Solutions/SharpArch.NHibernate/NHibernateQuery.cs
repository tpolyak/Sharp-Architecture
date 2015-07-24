namespace SharpArch.NHibernate
{
    using Domain;
    using global::NHibernate;

    public abstract class NHibernateQuery
    {
        private readonly ISession session;

        protected NHibernateQuery(ISession session)
        {
            Check.Require(session != null, "Session is required.");
            this.session = session;
        }

        protected virtual ISession Session
        {
            get
            {
                return this.session;
            }
        }
    }
}