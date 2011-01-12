using NHibernate;
using SharpArch.Core.DomainModel;
using System.Reflection;
using System;
using NHibernate.Criterion;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using System.Linq;

namespace SharpArch.Data.NHibernate
{
    public class EntityDuplicateChecker : IEntityDuplicateChecker
    {
        /// <summary>
        /// Provides a behavior specific repository for checking if a duplicate exists of an existing entity.
        /// </summary>
        public bool DoesDuplicateExistWithTypedIdOf<IdT>(IEntityWithTypedId<IdT> entity) {
            Check.Require(entity != null, "Entity may not be null when checking for duplicates");

			ISession session = GetSessionFor(entity);

            FlushMode previousFlushMode = session.FlushMode;

            // We do NOT want this to flush pending changes as checking for a duplicate should 
            // only compare the object against data that's already in the database
            session.FlushMode = FlushMode.Never;

            ICriteria criteria = session.CreateCriteria(entity.GetType())
                .Add(Expression.Not(Expression.Eq("Id", entity.Id)))
                .SetMaxResults(1);

            AppendSignaturePropertyCriteriaTo<IdT>(criteria, entity);
            bool doesDuplicateExist = criteria.List().Count > 0;
            session.FlushMode = previousFlushMode;
            return doesDuplicateExist;
        }

        private void AppendSignaturePropertyCriteriaTo<IdT>(ICriteria criteria, IEntityWithTypedId<IdT> entity) {
            foreach (PropertyInfo signatureProperty in entity.GetSignatureProperties()) {
                Type propertyType = signatureProperty.PropertyType;
                object propertyValue = signatureProperty.GetValue(entity, null);

                if (propertyType.IsEnum) {
                    criteria.Add(
                        Expression.Eq(signatureProperty.Name, (int) propertyValue));
                }
                else if (propertyType.GetInterfaces()
                    .Any(x => x.IsGenericType && 
                         x.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>))) {
                    AppendEntityCriteriaTo<IdT>(criteria, signatureProperty, propertyValue);
                }
                else if (propertyType == typeof(DateTime)) {
                    AppendDateTimePropertyCriteriaTo(criteria, signatureProperty, propertyValue);
                }
                else if (propertyType == typeof(String)) {
                    AppendStringPropertyCriteriaTo(criteria, signatureProperty, propertyValue);
                }
                else if (propertyType.IsValueType) {
                    AppendValuePropertyCriteriaTo(criteria, signatureProperty, propertyValue);
                }
                else {
                    throw new ApplicationException("Can't determine how to use " + entity.GetType() + "." +
                        signatureProperty.Name + " when looking for duplicate entries. To remedy this, " +
                        "you can create a custom validator or report an issue to the S#arp Architecture " +
                        "project, detailing the type that you'd like to be accommodated.");
                }
            }
        }

        private static void AppendStringPropertyCriteriaTo(ICriteria criteria, 
            PropertyInfo signatureProperty, object propertyValue) {
            if (propertyValue != null) {
                criteria.Add(
                    Expression.InsensitiveLike(signatureProperty.Name, propertyValue.ToString(), MatchMode.Exact));
            }
            else {
                criteria.Add(Expression.IsNull(signatureProperty.Name));
            }
        }

        private void AppendDateTimePropertyCriteriaTo(ICriteria criteria, 
            PropertyInfo signatureProperty, object propertyValue) {
            if ((DateTime)propertyValue > UNINITIALIZED_DATETIME) {
                criteria.Add(Expression.Eq(signatureProperty.Name, propertyValue));
            }
            else {
                criteria.Add(Expression.IsNull(signatureProperty.Name));
            }
        }

        private static void AppendValuePropertyCriteriaTo(ICriteria criteria, 
            PropertyInfo signatureProperty, object propertyValue) {
            if (propertyValue != null) {
                criteria.Add(Expression.Eq(signatureProperty.Name, propertyValue));
            }
            else {
                criteria.Add(Expression.IsNull(signatureProperty.Name));
            }
        }

        private static void AppendEntityCriteriaTo<IdT>(ICriteria criteria, 
            PropertyInfo signatureProperty, object propertyValue) {
            if (propertyValue != null) {
                criteria.Add(Expression.Eq(signatureProperty.Name + ".Id",
                    ((IEntityWithTypedId<IdT>)propertyValue).Id));
            }
            else {
                criteria.Add(Expression.IsNull(signatureProperty.Name + ".Id"));
            }
        }

		private ISession GetSessionFor(object entity)
		{
      string factoryKey = SessionFactoryKeyHelper.GetKey(entity);
			return NHibernateSession.CurrentFor(factoryKey);
		}

        private readonly DateTime UNINITIALIZED_DATETIME = default(DateTime);
    }
}
