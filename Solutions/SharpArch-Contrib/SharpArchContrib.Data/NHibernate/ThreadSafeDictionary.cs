using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SharpArchContrib.Data.NHibernate {
    /// <summary>
    /// Taken from http://devplanet.com/blogs/brianr/archive/2008/09/29/thread-safe-dictionary-update.aspx
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class ThreadSafeDictionary<TKey, TValue> : IThreadSafeDictionary<TKey, TValue> {
        //This is the internal dictionary that we are wrapping
        private readonly IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();


        [NonSerialized] private readonly ReaderWriterLockSlim dictionaryLock =
            Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        #region IThreadSafeDictionary<TKey,TValue> Members

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to remove</param>
        public void RemoveSafe(TKey key) {
            using (new ReadLock(dictionaryLock)) {
                if (dict.ContainsKey(key)) {
                    using (new WriteLock(dictionaryLock)) {
                        dict.Remove(key);
                    }
                }
            }
        }


        /// <summary>
        /// Merge does a blind remove, and then add.  Basically a blind Upsert.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        public void MergeSafe(TKey key, TValue newValue) {
            using (new WriteLock(dictionaryLock)) // take a writelock immediately since we will always be writing
            {
                if (dict.ContainsKey(key)) {
                    dict.Remove(key);
                }


                dict.Add(key, newValue);
            }
        }


        public virtual bool Remove(TKey key) {
            using (new WriteLock(dictionaryLock)) {
                return dict.Remove(key);
            }
        }


        public virtual bool ContainsKey(TKey key) {
            using (new ReadOnlyLock(dictionaryLock)) {
                return dict.ContainsKey(key);
            }
        }


        public virtual bool TryGetValue(TKey key, out TValue value) {
            using (new ReadOnlyLock(dictionaryLock)) {
                return dict.TryGetValue(key, out value);
            }
        }


        public virtual TValue this[TKey key] {
            get {
                using (new ReadOnlyLock(dictionaryLock)) {
                    return dict[key];
                }
            }
            set {
                using (new WriteLock(dictionaryLock)) {
                    dict[key] = value;
                }
            }
        }


        public virtual ICollection<TKey> Keys {
            get {
                using (new ReadOnlyLock(dictionaryLock)) {
                    return new List<TKey>(dict.Keys);
                }
            }
        }


        public virtual ICollection<TValue> Values {
            get {
                using (new ReadOnlyLock(dictionaryLock)) {
                    return new List<TValue>(dict.Values);
                }
            }
        }


        public virtual void Clear() {
            using (new WriteLock(dictionaryLock)) {
                dict.Clear();
            }
        }


        public virtual int Count {
            get {
                using (new ReadOnlyLock(dictionaryLock)) {
                    return dict.Count;
                }
            }
        }


        public virtual bool Contains(KeyValuePair<TKey, TValue> item) {
            using (new ReadOnlyLock(dictionaryLock)) {
                return dict.Contains(item);
            }
        }


        public virtual void Add(KeyValuePair<TKey, TValue> item) {
            using (new WriteLock(dictionaryLock)) {
                dict.Add(item);
            }
        }


        public virtual void Add(TKey key, TValue value) {
            using (new WriteLock(dictionaryLock)) {
                dict.Add(key, value);
            }
        }


        public virtual bool Remove(KeyValuePair<TKey, TValue> item) {
            using (new WriteLock(dictionaryLock)) {
                return dict.Remove(item);
            }
        }


        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            using (new ReadOnlyLock(dictionaryLock)) {
                dict.CopyTo(array, arrayIndex);
            }
        }


        public virtual bool IsReadOnly {
            get {
                using (new ReadOnlyLock(dictionaryLock)) {
                    return dict.IsReadOnly;
                }
            }
        }


        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            throw new NotSupportedException(
                "Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection");
        }


        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotSupportedException(
                "Cannot enumerate a threadsafe dictionary.  Instead, enumerate the keys or values collection");
        }

        #endregion
    }

    public static class Locks {
        public static void GetReadLock(ReaderWriterLockSlim locks) {
            bool lockAcquired = false;
            while (!lockAcquired) {
                lockAcquired = locks.TryEnterUpgradeableReadLock(1);
            }
        }


        public static void GetReadOnlyLock(ReaderWriterLockSlim locks) {
            bool lockAcquired = false;
            while (!lockAcquired) {
                lockAcquired = locks.TryEnterReadLock(1);
            }
        }


        public static void GetWriteLock(ReaderWriterLockSlim locks) {
            bool lockAcquired = false;
            while (!lockAcquired) {
                lockAcquired = locks.TryEnterWriteLock(1);
            }
        }


        public static void ReleaseReadOnlyLock(ReaderWriterLockSlim locks) {
            if (locks.IsReadLockHeld) {
                locks.ExitReadLock();
            }
        }


        public static void ReleaseReadLock(ReaderWriterLockSlim locks) {
            if (locks.IsUpgradeableReadLockHeld) {
                locks.ExitUpgradeableReadLock();
            }
        }


        public static void ReleaseWriteLock(ReaderWriterLockSlim locks) {
            if (locks.IsWriteLockHeld) {
                locks.ExitWriteLock();
            }
        }


        public static void ReleaseLock(ReaderWriterLockSlim locks) {
            ReleaseWriteLock(locks);
            ReleaseReadLock(locks);
            ReleaseReadOnlyLock(locks);
        }


        public static ReaderWriterLockSlim GetLockInstance() {
            return GetLockInstance(LockRecursionPolicy.SupportsRecursion);
        }


        public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy recursionPolicy) {
            return new ReaderWriterLockSlim(recursionPolicy);
        }
    }

    public abstract class BaseLock : IDisposable {
        [CLSCompliant(false)]
        protected ReaderWriterLockSlim _Locks;


        public BaseLock(ReaderWriterLockSlim locks) {
            _Locks = locks;
        }

        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }

    public class ReadLock : BaseLock {
        public ReadLock(ReaderWriterLockSlim locks)
            : base(locks) {
            Locks.GetReadLock(_Locks);
        }


        public override void Dispose() {
            Locks.ReleaseReadLock(_Locks);
        }
    }

    public class ReadOnlyLock : BaseLock {
        public ReadOnlyLock(ReaderWriterLockSlim locks)
            : base(locks) {
            Locks.GetReadOnlyLock(_Locks);
        }


        public override void Dispose() {
            Locks.ReleaseReadOnlyLock(_Locks);
        }
    }

    public class WriteLock : BaseLock {
        public WriteLock(ReaderWriterLockSlim locks)
            : base(locks) {
            Locks.GetWriteLock(_Locks);
        }


        public override void Dispose() {
            Locks.ReleaseWriteLock(_Locks);
        }
    }
}