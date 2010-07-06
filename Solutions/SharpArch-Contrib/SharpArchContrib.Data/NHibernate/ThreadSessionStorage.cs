using System;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using SharpArch.Data.NHibernate;

namespace SharpArchContrib.Data.NHibernate {
    public class ThreadSessionStorage : IUnitOfWorkSessionStorage {
        private readonly ThreadSafeDictionary<string, SimpleSessionStorage> perThreadSessionStorage =
            new ThreadSafeDictionary<string, SimpleSessionStorage>();

        #region IUnitOfWorkSessionStorage Members

        public IEnumerable<ISession> GetAllSessions() {
            return GetSimpleSessionStorageForThread().GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey) {
            return GetSimpleSessionStorageForThread().GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session) {
            GetSimpleSessionStorageForThread().SetSessionForKey(factoryKey, session);
        }

        public void EndUnitOfWork(bool closeSessions) {
            if (closeSessions) {
                NHibernateSession.CloseAllSessions();
                perThreadSessionStorage.Remove(GetCurrentThreadName());
            }
            else {
                foreach (ISession session in GetAllSessions()) {
                    session.Clear();
                }
            }
        }

        #endregion

        private SimpleSessionStorage GetSimpleSessionStorageForThread() {
            string currentThreadName = GetCurrentThreadName();
            SimpleSessionStorage sessionStorage;
            if (!perThreadSessionStorage.TryGetValue(currentThreadName, out sessionStorage)) {
                sessionStorage = new SimpleSessionStorage();
                perThreadSessionStorage.Add(currentThreadName, sessionStorage);
            }

            return sessionStorage;
        }

        private string GetCurrentThreadName() {
            if (Thread.CurrentThread.Name == null) {
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            }
            return Thread.CurrentThread.Name;
        }
    }
}