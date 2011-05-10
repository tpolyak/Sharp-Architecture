namespace SharpArch.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;

    public class SimpleSessionStorage : ISessionStorage
    {
        private readonly Dictionary<string, ISession> storage = new Dictionary<string, ISession>();

        /// <summary>
        ///     Returns all the values of the internal dictionary of sessions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISession> GetAllSessions()
        {
            return this.storage.Values;
        }

        /// <summary>
        ///     Returns the session associated with the specified factoryKey or
        ///     null if the specified factoryKey is not found.
        /// </summary>
        /// <param name = "factoryKey"></param>
        /// <returns></returns>
        public ISession GetSessionForKey(string factoryKey)
        {
            ISession session;

            if (!this.storage.TryGetValue(factoryKey, out session))
            {
                return null;
            }

            return session;
        }

        /// <summary>
        ///     Stores the session into a dictionary using the specified factoryKey.
        ///     If a session already exists by the specified factoryKey, 
        ///     it gets overwritten by the new session passed in.
        /// </summary>
        /// <param name = "factoryKey"></param>
        /// <param name = "session"></param>
        public void SetSessionForKey(string factoryKey, ISession session)
        {
            this.storage[factoryKey] = session;
        }
    }
}