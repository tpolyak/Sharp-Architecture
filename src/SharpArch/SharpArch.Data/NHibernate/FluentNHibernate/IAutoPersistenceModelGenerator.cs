using FluentNHibernate.AutoMap;
using System;

namespace SharpArch.Data.NHibernate.FluentNHibernate
{
	[CLSCompliant(false)]
	public interface IAutoPersistenceModelGenerator
    {
        AutoPersistenceModel Generate();
    }
}
