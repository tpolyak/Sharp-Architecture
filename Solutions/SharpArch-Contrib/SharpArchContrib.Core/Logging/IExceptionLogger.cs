using System;

namespace SharpArchContrib.Core.Logging {
    public interface IExceptionLogger {
        void LogException(Exception err, bool isSilent, Type throwingType);
    }
}