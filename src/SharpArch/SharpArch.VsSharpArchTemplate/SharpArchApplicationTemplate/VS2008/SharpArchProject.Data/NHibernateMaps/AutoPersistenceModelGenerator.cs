using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using $solutionname$.Core;
using $safeprojectname$.NHibernateMaps.Conventions;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace $safeprojectname$.NHibernateMaps
{

    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {

        #region IAutoPersistenceModelGenerator Members

        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<Class1>(new AutomappingConfiguration())
                .Conventions.Setup(GetConventions())
                .IgnoreBase<Entity>()
                .IgnoreBase(typeof(EntityWithTypedId<>))
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        #endregion

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.ForeignKeyConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.HasManyConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.HasManyToManyConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.ManyToManyTableNameConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.PrimaryKeyConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.ReferenceConvention>();
                c.Add<$solutionname$.Data.NHibernateMaps.Conventions.TableNameConvention>();
            };
        }
    }
}
