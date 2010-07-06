using System;
using log4net;

namespace SharpArchContrib.Core.Logging {
    public static class Log4NetHelper {
        public static void Log(this ILog logger, LoggingLevel level, string message) {
            switch (level) {
                case LoggingLevel.All:
                case LoggingLevel.Debug:
                    logger.Debug(message);
                    break;
                case LoggingLevel.Info:
                    logger.Info(message);
                    break;
                case LoggingLevel.Warn:
                    logger.Warn(message);
                    break;
                case LoggingLevel.Error:
                    logger.Error(message);
                    break;
                case LoggingLevel.Fatal:
                    logger.Fatal(message);
                    break;
            }
        }

        public static void Log(this ILog logger, LoggingLevel level, string message, Exception err) {
            switch (level) {
                case LoggingLevel.All:
                case LoggingLevel.Debug:
                    if (message == null) {
                        logger.Debug(err);
                    }
                    else {
                        logger.Debug(message, err);
                    }
                    break;
                case LoggingLevel.Info:
                    if (message == null) {
                        logger.Info(err);
                    }
                    else {
                        logger.Info(message, err);
                    }
                    break;
                case LoggingLevel.Warn:
                    if (message == null) {
                        logger.Warn(err);
                    }
                    else {
                        logger.Warn(message, err);
                    }
                    break;
                case LoggingLevel.Error:
                    if (message == null) {
                        logger.Error(err);
                    }
                    else {
                        logger.Error(message, err);
                    }
                    break;
                case LoggingLevel.Fatal:
                    if (message == null) {
                        logger.Fatal(err);
                    }
                    else {
                        logger.Fatal(message, err);
                    }
                    break;
            }
        }

        public static bool IsEnabledFor(this ILog logger, LoggingLevel level) {
            switch (level) {
                case LoggingLevel.All:
                    return true;
                case LoggingLevel.Debug:
                    return logger.IsDebugEnabled;
                case LoggingLevel.Info:
                    return logger.IsInfoEnabled;
                case LoggingLevel.Warn:
                    return logger.IsWarnEnabled;
                case LoggingLevel.Error:
                    return logger.IsErrorEnabled;
                case LoggingLevel.Fatal:
                    return logger.IsFatalEnabled;
            }

            return false;
        }
    }
}