using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Runtime.Remoting;
using System.Collections;
using ProjectBase.Core;
using ProjectBase.Core.PersistenceSupport;

namespace ProjectBase.Web
{ 
	public class ControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(Type controllerType) {
			foreach (ConstructorInfo ci in controllerType.GetConstructors()) {
				ArrayList list = new ArrayList();

                foreach (ParameterInfo pi in ci.GetParameters()) {
					Type type = pi.ParameterType;
					ConcreteTypeAttribute attr =
						Attribute.GetCustomAttribute(type, typeof(ConcreteTypeAttribute)) as ConcreteTypeAttribute;
					Type targetType = Type.GetType(attr.TypeName);

                    if (targetType.ContainsGenericParameters) {
						Type[] generics = type.GetGenericArguments();
						targetType = targetType.MakeGenericType(generics);
					}

					object arg = Activator.CreateInstance(targetType);
					list.Add(arg);
				}

                object[] args = list.ToArray();
				
                return Activator.CreateInstance(controllerType, args) as IController;
			}

            return base.GetControllerInstance(controllerType);
		}
	}
}
