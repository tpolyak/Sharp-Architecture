using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpArch.Core;
using SharpArchContrib.Core;

namespace Tests.SharpArchContrib.Core
{
    [TestFixture]
    public class ParameterCheckTests
    {
        [Test]
        public void DictionaryContainKey_Supports_Dictionary_With_String_Key_And_Value(){
            Dictionary<string, string> dict = new Dictionary<string,string>();
            Assert.Throws<PreconditionException>(() => ParameterCheck.DictionaryContainsKey(dict, "dict", "key"));
        }
    }
}
