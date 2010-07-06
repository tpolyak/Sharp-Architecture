using System;
using System.Reflection;
using NUnit.Framework;
using SharpArch.Testing.NUnit;
using SharpArchContrib.Castle.NHibernate;

namespace Tests.SharpArchContrib.Castle.NHibernate {
    [TestFixture]
    public class TransactionAttributeTests {
        public class TestClass {
            [Transaction]
            [UnitOfWork]
            public void Transaction() {}

            [UnitOfWork]
            [Transaction]
            public void UnitOfWork() {}
        }

        [Test]
        public void Gathering_Transaction_Attributes_Does_Not_Gather_UnitOfWork() {
            foreach (MethodInfo methodInfo in typeof(TestClass).GetMethods()) {
                if (methodInfo.Name == "Transaction" || methodInfo.Name == "UnitOfWork") {
                    var attributes = (Attribute[]) methodInfo.GetCustomAttributes(typeof(TransactionAttribute), false);
                    attributes.Length.ShouldEqual(1);
                    foreach (Attribute attribute in attributes) {
                        attribute.ShouldBeOfType(typeof(TransactionAttribute));
                        attribute.ShouldNotBeOfType(typeof(UnitOfWorkAttribute));
                    }
                }
            }
        }
    }
}