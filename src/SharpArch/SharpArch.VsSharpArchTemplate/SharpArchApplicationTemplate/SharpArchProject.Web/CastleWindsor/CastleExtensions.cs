using Castle.MicroKernel.Registration;
using System;
using System.Linq;

namespace $safeprojectname$.CastleWindsor
{
    public static class CastleExtensions
    {
        /// <summary>
        /// Searches for the first interface found associated with the 
        /// <see cref="ServiceDescriptor" /> which is not generic and which 
        /// is found in the specified namespace.
        /// </summary>
        /// <remarks>This could be moved to a reusable class library, e.g., SharpArch.CastleWindsor, 
        /// but I don't see the justification for just one extension method.</remarks>
        public static BasedOnDescriptor FirstNonGenericCoreInterface(this ServiceDescriptor descriptor, string interfaceNamespace) {
            return descriptor.Select(delegate(Type type, Type baseType) {
                var interfaces = type.GetInterfaces()
                    .Where(
                    t => t.IsGenericType == false && t.Namespace.StartsWith(interfaceNamespace)
                    );
                
                if (interfaces.Count() > 0) {
                    return new[] { interfaces.ElementAt(0) };
                }
                
                return null;
            });
        }
    }
}
