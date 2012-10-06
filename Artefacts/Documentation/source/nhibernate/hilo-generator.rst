NHibernate HiLo Generator
=========================

Why use HiLo
------------

TODO

Using HiLo Id generation
------------------------
In your Infrastructure project, under NHibernateMaps:

::

    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "Id");
            instance.UnsavedValue("0");
            instance.GeneratedBy.HiLo("1000");
        }
    }

Create the following table:

::

    CREATE TABLE [dbo].[hibernate_unique_key](
           [next_hi] [int] NOT NULL
            ) ON [PRIMARY]

Populate the column with a number to seed and you're done.
