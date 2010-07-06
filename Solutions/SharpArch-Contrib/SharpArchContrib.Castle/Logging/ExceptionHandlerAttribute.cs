using System;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.Castle.Logging {
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    public class ExceptionHandlerAttribute : Attribute {
        public ExceptionHandlerAttribute() {
            Settings = new ExceptionHandlerAttributeSettings();
        }

        public bool IsSilent {
            get { return Settings.IsSilent; }
            set { Settings.IsSilent = value; }
        }

        public object ReturnValue {
            get { return Settings.ReturnValue; }
            set { Settings.ReturnValue = value; }
        }

        public Type ExceptionType
        {
            get { return Settings.ExceptionType; }
            set { Settings.ExceptionType = value; }
        }

        public ExceptionHandlerAttributeSettings Settings { get; set; }
    }
}