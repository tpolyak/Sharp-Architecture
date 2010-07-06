using System;
using NUnit.Framework;

namespace Tests {
    public static class SyntaxHelper {
        public static void ShouldNotContain(this string actual, string expected) {
            StringAssert.DoesNotContain(expected, actual);
        }

        public static void ShouldEqualInMemorySqlDateTime(this DateTime actual, DateTime expected) {
            DateTime expectedWithoutMilliseconds = actual.AddMilliseconds(-1*actual.Millisecond);
            Assert.AreEqual(actual, expectedWithoutMilliseconds);
        }
    }
}