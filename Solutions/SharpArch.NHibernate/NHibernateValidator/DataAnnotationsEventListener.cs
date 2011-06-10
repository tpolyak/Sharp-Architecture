namespace SharpArch.NHibernate.NHibernateValidator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using global::NHibernate.Event;

    using SharpArch.Domain.DomainModel;

    [Serializable]
    internal class DataAnnotationsEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var entity = (Entity)@event.Entity;
            Validator.ValidateObject(entity, new ValidationContext(entity, null, null), true);
            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var entity = (Entity)@event.Entity;
            Validator.ValidateObject(entity, new ValidationContext(entity, null, null), true);
            return false;
        }
    }
}
