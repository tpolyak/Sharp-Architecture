﻿namespace SharpArch.Testing.NUnit
{
    using System;

    using global::NUnit.Framework;

    /// <summary>
    ///     Provides a base class for BDD unit tests, as described at http://flux88.com/blog/the-transition-from-tdd-to-bdd/.
    ///     This is an optional base class which need not be used with unit tests.
    /// </summary>
    public abstract class BehaviorSpecificationTestsBase
    {
        protected Exception ExceptionThrown { get; private set; }

        /// <summary>
        ///     Perform actions upon the model.  Separate test methods would then be employed to verify
        ///     the results of the system under test.
        /// </summary>
        protected abstract void Act();

        /// <summary>
        ///     Method used to setup the model that will be tested.  This includes creating mock objects,
        ///     preparing the model properties, preparing the database, etc.
        /// </summary>
        protected abstract void EstablishContext();

        [SetUp]
        protected virtual void SetUp()
        {
            this.EstablishContext();

            try
            {
                this.Act();
            }
            catch (Exception exc)
            {
                this.ExceptionThrown = exc;
            }
        }
    }
}