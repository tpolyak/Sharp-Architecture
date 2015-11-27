using System;
using NHibernate.Event;

namespace Tests.SharpArch.NHibernate
{
    [Serializable]
    public class PreInsertListener : IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            return true;
        }
    }

    [Serializable]
    public class PreUpdateListener : IPreUpdateEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return true;
        }
    }
}