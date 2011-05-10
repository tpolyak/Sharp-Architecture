namespace SharpArch.Domain.PersistenceSupport
{
    using SharpArch.Domain.DomainModel;

    public interface IEntityDuplicateChecker
    {
        bool DoesDuplicateExistWithTypedIdOf<TId>(IEntityWithTypedId<TId> entity);
    }
}