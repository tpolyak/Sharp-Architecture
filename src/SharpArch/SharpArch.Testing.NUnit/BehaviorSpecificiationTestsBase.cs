using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SharpArch.Testing.NUnit
{
    /// <summary>
    /// Provides a base class for BDD unit tests, as described at http://flux88.com/blog/the-transition-from-tdd-to-bdd/.
    /// This is an optional base class which need not be used with unit tests.
    /// </summary>
    public abstract class BehaviorSpecificiationTestsBase
    {
        protected Exception ExceptionThrown { get; private set; }

        /// <summary>
        /// Method used to setup the model that will be tested.  This includes creating mock objects,
        /// preparing the model properties, preparing the database, etc.
        /// </summary>
        protected abstract void EstablishContext();

        /// <summary>
        /// Perform actions upon the model.  Separate test methods would then be employed to verify
        /// the results of the system under test.
        /// </summary>
        protected abstract void Act();

        [SetUp]
        protected virtual void SetUp() {
            EstablishContext();

            try {
                Act();
            }
            catch (Exception exc) {
                ExceptionThrown = exc;
            }
        }
    }
}
