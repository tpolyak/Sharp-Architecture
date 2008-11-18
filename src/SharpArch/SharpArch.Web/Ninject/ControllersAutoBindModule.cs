using Ninject.Core;
using System.Web.Mvc;
using Ninject.Conditions;
using System.Reflection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SharpArch.Web.Ninject
{
    public class ControllersAutoBindModule : StandardModule
    {
        public ControllersAutoBindModule(string controllersAssemblyName) {
            this.controllersAssemblyName = controllersAssemblyName;
        }

        public override void Load() {
            BindControllers();
        }

        /// <summary>
        /// Retrieves all controllers from the provided assembly and binds
        /// them to the respective controller name
        /// </summary>
        private void BindControllers() {
            Assembly controllersAssembly = Assembly.Load(controllersAssemblyName);

            IEnumerable<Type> controllers =
                from controller in controllersAssembly.GetTypes()
                where controller.IsSubclassOf(typeof(Controller))
                    && !controller.IsAbstract
                select controller;

            foreach (Type controller in controllers) {
                Bind<IController>().To(controller).Only(
                    When.Context.Variable("controllerName")
                    .EqualTo(controller.Name.Replace("Controller", ""), StringComparer.OrdinalIgnoreCase));
            }
        }

        private readonly string controllersAssemblyName;
    }
}
