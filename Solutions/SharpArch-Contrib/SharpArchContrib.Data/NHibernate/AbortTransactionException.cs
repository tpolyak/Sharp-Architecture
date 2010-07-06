using System;
using System.Runtime.Serialization;

namespace SharpArchContrib.Data.NHibernate {
    [Serializable]
    public class AbortTransactionException : Exception {
        public AbortTransactionException() {}

        public AbortTransactionException(string message) : base(message) {}

        public AbortTransactionException(string message, Exception inner) : base(message, inner) {}

        protected AbortTransactionException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}
    }
}