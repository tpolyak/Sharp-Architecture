using Northwind.Core;

namespace Tests.Northwind.Core
{
    /// <summary>
    /// Since region has a protected constructor in the domain, since it's a reference type from the DB,
    /// this child object serves as a means to perform testing with custom created regions.
    /// </summary>
    internal class RegionWithPublicConstructor : Region
    {
        public RegionWithPublicConstructor(string regionDesc)
            : base() {
            Description = regionDesc;
        }
    }
}
