namespace Suteki.TardisBank.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using SharpArch.Domain.Commands;
    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Tasks.EventHandlers;

    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromAssemblyContaining<SendMessageEmailHandler>()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface().LifestyleTransient());

            container.Register(
                AllTypes.FromAssemblyContaining<SendMessageEmailHandler>()
                    .BasedOn(typeof(IHandles<>))
                    .WithService.FirstInterface().LifestyleTransient());
        }
    }
}