using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpArch.Core.PersistenceSupport
{
    public interface IDbContext
    {
        void CommitChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
