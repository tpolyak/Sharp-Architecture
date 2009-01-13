using SharpArch.Core.DomainModel;

namespace SharpArch.Core.PersistenceSupport
{
    public interface IEntityDuplicateChecker
    {
        bool DoesDuplicateExistWithTypedIdOf<IdT>(IEntityWithTypedId<IdT> entity);
    }
}
