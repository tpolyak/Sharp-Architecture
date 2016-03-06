namespace SharpArch.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///     This is a helper for accessing dependencies via the Common Service Locator (CSL). But while
    ///     the CSL will throw object reference errors if used before initialization, this will inform
    ///     you of what the problem is. Perhaps it would be more aptly named "InformativeServiceLocator".
    /// </summary>
    /// <typeparam name="TDependency">The dependency type.</typeparam>
    [Obsolete("Remove in favour of constructor or property injection.")]
    [ExcludeFromCodeCoverage]
    [PublicAPI]
    public static class SafeServiceLocator<TDependency>
    {
        /// <summary>
        ///     Returns the service.
        /// </summary>
        /// <returns>A service.</returns>
        /// <exception cref="NullReferenceException">ServiceLocator was not initialized.</exception>
        /// <exception cref="ActivationException">Requested service is not registered or cannot be instantiated.</exception>
        public static TDependency GetService()
        {
            TDependency service;

            try
            {
                service = (TDependency)ServiceLocator.Current.GetService(typeof(TDependency));
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(
                    "ServiceLocator has not been initialized; I was trying to retrieve " + typeof(TDependency),
                    ex);
            }
            catch (ActivationException ex)
            {
                throw new ActivationException(
                    "The needed dependency of type " + typeof(TDependency).Name +
                    " could not be located with the ServiceLocator. You'll need to register it with " +
                    "the Common Service Locator (CSL) via your IoC's CSL adapter. " + ex.Message, ex);
            }

            return service;
        }
    }
}