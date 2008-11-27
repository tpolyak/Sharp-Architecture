using SharpArch.Core.PersistenceSupport;
using System.Reflection;
using SharpArch.Core;

namespace SharpArch.Testing
{
    /// <summary>
    /// For better data integrity, it is imperitive that the <see cref="PersistentObject.ID"/>
    /// property is read-only and set only by the ORM.  With that said, some unit tests need 
    /// ID set to a particular value; therefore, this utility enables that ability.  This class should 
    /// never be used outside of the testing project; instead, implement <see cref="IHasAssignedId" /> to 
    /// expose a public setter.
    /// </summary>
    public class PersistentObjectIdSetter<IdT>
    {
        /// <summary>
        /// Uses reflection to set the ID of a <see cref="PersistentObjectWithTypedId" />.
        /// </summary>
        public static void SetIdOf(IPersistentObjectWithTypedId<IdT> persistentObject, IdT id) {
            // Set the data property reflectively
            PropertyInfo idProperty = persistentObject.GetType().GetProperty("ID",
                BindingFlags.Public | BindingFlags.Instance);

            Check.Ensure(idProperty != null, "idProperty could not be found");

            idProperty.SetValue(persistentObject, id, null);
        }
    }
}
