namespace Tests.SharpArch.Domain.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using global::SharpArch.Domain.Commands;

    [TestFixture]
    public class CommandProcessorTests
    {
        private IList<ICommand> handledCommands;
        private IWindsorContainer container;
        private ICommandProcessor commandProcessor;

        [SetUp]
        public void SetUp()
        {
            handledCommands = new List<ICommand>();
            container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
            commandProcessor = new CommandProcessor();
        }

        [TearDown]
        public void TearDown()
        {
            handledCommands = null;

            commandProcessor = null;

            container.Dispose();
            container = null;
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsIfCommandIsInvalid()
        {
            var testCommand = new InvalidCommand();
            commandProcessor.Process(testCommand);
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsIfCommandIsInvalidButNotRequired()
        {
          var testCommand = new InvalidCommand { Invalid = true };
          commandProcessor.Process(testCommand);
        }

        [Test]
        [ExpectedException(typeof(CommandHandlerNotFoundException))]
        public void ThrowsIfCommandHandlerNotFound()
        {
            var testCommand = new TestCommand();
            commandProcessor.Process(testCommand);
        }

        [Test]
        public void CanHandleSingleCommand()
        {
            container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .LifeStyle.Transient);

            var testCommand = new TestCommand();
            commandProcessor.Process(testCommand);

            Assert.AreEqual(1, handledCommands.Count);
            Assert.AreEqual(testCommand, handledCommands[0]);
        }

        [Test]
        public void CanHandleMultipleCommands()
        {
            container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .Named("First handler")
                .LifeStyle.Transient);

            container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .Named("Second handler")
                .LifeStyle.Transient);

            var testCommand = new TestCommand();
            commandProcessor.Process(testCommand);

            Assert.AreEqual(2, handledCommands.Count);
            Assert.AreEqual(testCommand, handledCommands[0]);
            Assert.AreEqual(testCommand, handledCommands[1]);
        }

        private TestCommandHandler<TCommand> CreateCommandHandler<TCommand>() where TCommand : ICommand
        {
            return new TestCommandHandler<TCommand>(OnHandle);
        }

        private void OnHandle<TCommand>(ICommandHandler<TCommand> handler, TCommand command) where TCommand : ICommand
        {
            handledCommands.Add(command);
        }
    }
}