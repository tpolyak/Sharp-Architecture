using System;
using log4net;

namespace SharpArchContrib.Core.Logging {
    public class ExceptionLogger : IExceptionLogger {
        #region IExceptionLogger Members

        public void LogException(Exception err, bool isSilent, Type throwingType) {
            ILog logger = LogManager.GetLogger(throwingType);
            string message = null;
            if (isSilent) {
                message = "[SILENT]";
            }
            logger.Log(LoggingLevel.Error, message, err);
        }

        #endregion
    }
}