using System.Collections.Generic;

namespace SharpArchContrib.Data.NHibernate {
    /// <summary>
    /// Taken from http://devplanet.com/blogs/brianr/archive/2008/09/29/thread-safe-dictionary-update.aspx
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
        /// <summary>
        /// Merge is similar to the SQL merge or upsert statement.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        void MergeSafe(TKey key, TValue newValue);


        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to Remove</param>
        void RemoveSafe(TKey key);
    }
}