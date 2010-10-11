namespace SharpArch.Core.PersistenceSupport
{
    using SharpArch.Core.DomainModel;

    public interface IEntityDuplicateChecker
    {
        bool DoesDuplicateExistWithTypedIdOf<TId>(IEntityWithTypedId<TId> entity);
    }
}