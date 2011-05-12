namespace Tests.SharpArch.Domain.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using global::SharpArch.Domain.Commands;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    [TestFixture]
    public class CommandProcessorTests
    {
        private IList<ICommand> _handledCommands;
        private IWindsorContainer _container;
        private ICommandProcessor _commandProcessor;

        [SetUp]
        public void SetUp()
        {
            _handledCommands = new List<ICommand>();
            _container = new WindsorContainer();
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
            _commandProcessor = new CommandProcessor();
        }

        [TearDown]
        public void TearDown()
        {
            _handledCommands = null;

            _commandProcessor = null;

            _container.Dispose();
            _container = null;
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void ThrowsIfCommandIsInvalid()
        {
            var testCommand = new InvalidCommand();
            _commandProcessor.Process(testCommand);
        }

        [Test]
        [ExpectedException(typeof(CommandHandlerNotFoundException))]
        public void ThrowsIfCommandHandlerNotFound()
        {
            var testCommand = new TestCommand();
            _commandProcessor.Process(testCommand);
        }

        [Test]
        public void CanHandleSingleCommand()
        {
            _container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .LifeStyle.Transient);

            var testCommand = new TestCommand();
            _commandProcessor.Process(testCommand);

            Assert.AreEqual(1, _handledCommands.Count);
            Assert.AreEqual(testCommand, _handledCommands[0]);
        }

        [Test]
        public void CanHandleMultipleCommands()
        {
            _container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .Named("First handler")
                .LifeStyle.Transient);

            _container.Register(
              Component.For<ICommandHandler<TestCommand>>()
                .UsingFactoryMethod(CreateCommandHandler<TestCommand>)
                .Named("Second handler")
                .LifeStyle.Transient);

            var testCommand = new TestCommand();
            var results = _commandProcessor.Process(testCommand);

            Assert.AreEqual(2, _handledCommands.Count);
            Assert.AreEqual(testCommand, _handledCommands[0]);
            Assert.AreEqual(testCommand, _handledCommands[1]);

            Assert.IsTrue(results.Success);
            Assert.AreEqual(2, results.Results.Length);
            Assert.IsTrue(results.Results[0].Success);
            Assert.IsTrue(results.Results[1].Success);
        }

        private TestCommandHandler<TCommand> CreateCommandHandler<TCommand>() where TCommand : ICommand
        {
            return new TestCommandHandler<TCommand>(OnHandle);
        }

        private ICommandResult OnHandle<TCommand>(ICommandHandler<TCommand> handler, TCommand command) where TCommand : ICommand
        {
            _handledCommands.Add(command);
            return new TestCommandResult(true);
        }
    }
}