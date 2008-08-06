namespace ProjectBase.Core.PersistenceSupport
{
    public interface IHasAssignedId<IdT>
    {
        /// <summary>
        /// Enables developer to set the assigned ID of an object.  This is not part of 
        /// <see cref="DomainObject{IdT}" /> since most domain objects do not have assigned 
        /// IDs and since business rules will certainly vary as to what constitutes a valid,
        /// assigned ID for one object but not for another.
        /// </summary>
        void SetAssignedIdTo(IdT assignedId);
    }
}
