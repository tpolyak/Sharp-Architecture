using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel;

namespace SharpArchContrib.Castle.Logging {
    public class LogFacility : AttributeControlledFacilityBase {
        public LogFacility() : base(typeof(LogInterceptor), LifestyleType.Singleton) {}

        protected override List<Attribute> GetAttributes(IHandler handler) {
            var attributes = new List<Attribute>();
            AddAssemblyLevelAttributes(attributes, handler);
            AddClassLevelAttributes(attributes, handler);
            AddMethodLevelAttributes(attributes, handler);

            return attributes;
        }

        private void AddMethodLevelAttributes(List<Attribute> attributes, IHandler handler) {
            foreach (MethodInfo methodInfo in handler.ComponentModel.Implementation.GetMethods()) {
                attributes.AddRange((Attribute[]) methodInfo.GetCustomAttributes(typeof(LogAttribute), false));
            }
        }

        private void AddClassLevelAttributes(List<Attribute> attributes, IHandler handler) {
            attributes.AddRange(
                (Attribute[]) handler.ComponentModel.Implementation.GetCustomAttributes(typeof(LogAttribute), false));
        }

        private void AddAssemblyLevelAttributes(List<Attribute> attributes, IHandler handler) {
            attributes.AddRange(
                (Attribute[])
                handler.ComponentModel.Implementation.Assembly.GetCustomAttributes(typeof(LogAttribute), false));
        }
    }
}