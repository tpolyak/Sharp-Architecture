namespace Suteki.TardisBank.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Suteki.TardisBank.Tasks;

    public class TasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                    .FromAssemblyContaining<UserService>()
                    .Where(Component.IsInSameNamespaceAs<UserService>())
                    .WithService.DefaultInterfaces()
                    .Configure(c => c.LifestyleTransient())
                );
        }
    }
}