using Castle.Windsor;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.Castle;
using Castle.MicroKernel.Registration;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

namespace $safeprojectname$.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container) {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddApplicationServicesTo(container);

            container.AddComponent("validator", 
                typeof(IValidator), typeof(Validator));
        }

        private static void AddApplicationServicesTo(IWindsorContainer container) {
            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("$solutionname$.ApplicationServices")
                .WithService.FirstInterface());
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container) {
            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("$solutionname$.Data")
                .WithService.FirstNonGenericCoreInterface("$solutionname$.Core"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container) {
            container.AddComponent("entityDuplicateChecker",
                typeof(IEntityDuplicateChecker), typeof(EntityDuplicateChecker));
            container.AddComponent("repositoryType",
                typeof(IRepository<>), typeof(Repository<>));
            container.AddComponent("nhibernateRepositoryType",
                typeof(INHibernateRepository<>), typeof(NHibernateRepository<>));
            container.AddComponent("repositoryWithTypedId",
                typeof(IRepositoryWithTypedId<,>), typeof(RepositoryWithTypedId<,>));
            container.AddComponent("nhibernateRepositoryWithTypedId",
                typeof(INHibernateRepositoryWithTypedId<,>), typeof(NHibernateRepositoryWithTypedId<,>));
        }
    }
}
