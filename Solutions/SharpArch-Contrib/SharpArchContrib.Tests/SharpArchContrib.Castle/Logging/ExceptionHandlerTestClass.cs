using System;
using SharpArchContrib.Castle.Logging;
using SharpArchContrib.Core.Logging;

namespace Tests.SharpArchContrib.Castle.Logging {
    public class ExceptionHandlerTestClass : IExceptionHandlerTestClass {
        #region IExceptionHandlerTestClass Members

        [ExceptionHandler]
        public void ThrowException() {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public float ThrowExceptionSilentWithReturn() {
            throw new NotImplementedException();
            return 7f;
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public void ThrowExceptionSilent() {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        [Log(ExceptionLevel = LoggingLevel.Error)]
        public float ThrowExceptionSilentWithReturnWithLogAttribute() {
            throw new NotImplementedException();
            return 7f;
        }

        [ExceptionHandler(ExceptionType = typeof(NotImplementedException))]
        public float ThrowBaseExceptionNoCatch()
        {
            throw new Exception();
            return 7f;
        }

        [ExceptionHandler(IsSilent=true, ExceptionType = typeof(NotImplementedException), ReturnValue = 6f)]
        public float ThrowNotImplementedExceptionCatch()
        {
            throw new NotImplementedException();
            return 7f;
        }

        #endregion
    }
}