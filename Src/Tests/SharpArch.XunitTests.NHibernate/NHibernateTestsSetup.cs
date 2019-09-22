namespace Tests.SharpArch.NHibernate
{
    using System.Reflection;
    using Domain;
    using global::SharpArch.Testing.NHibernate;
    using Mappings;


    public class NHibernateTestsSetup : TestDatabaseSetup
    {
        public NHibernateTestsSetup()
            : base(Assembly.GetExecutingAssembly().CodeBase,
                new[]
                {
                    typeof(ObjectWithGuidId).Assembly,
                    typeof(TestsPersistenceModelGenerator).Assembly
                })
        {
        }
    }
}
