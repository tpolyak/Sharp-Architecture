using System;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SharpArch.Testing.NUnit;

namespace Tests.SharpArchContrib.Castle.Logging {
    [TestFixture]
    public class ExceptionHandlerAttributeTests {
        #region Setup/Teardown

        [SetUp]
        public void SetUp() {
            exceptionHandlerTestClass = ServiceLocator.Current.GetInstance<IExceptionHandlerTestClass>();
        }

        #endregion

        private IExceptionHandlerTestClass exceptionHandlerTestClass;

        [Test]
        public void LoggedExceptionDoesNotRethrow() {
            Assert.DoesNotThrow(() => exceptionHandlerTestClass.ThrowExceptionSilent());
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturn() {
            exceptionHandlerTestClass.ThrowExceptionSilentWithReturn().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturnWithLogAttribute() {
            exceptionHandlerTestClass.ThrowExceptionSilentWithReturnWithLogAttribute().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionRethrows() {
            Assert.Throws<NotImplementedException>(() => exceptionHandlerTestClass.ThrowException());
        }

        [Test]
        public void ThrowBaseExceptionNoCatch()
        {
            Assert.Throws<Exception>(() => exceptionHandlerTestClass.ThrowBaseExceptionNoCatch());
        }

        [Test]
        public void ThrowNotImplementedExceptionCatch()
        {
            exceptionHandlerTestClass.ThrowNotImplementedExceptionCatch().ShouldEqual(6f);
        }
    }
}