namespace SharpArch.Domain.Events
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///     Manages domain events.
    /// </summary>
    public class DomainEvents
	{
        [ThreadStatic]
        private static List<Delegate> actions;

        /// <summary>
        ///     Gets or sets the service locator.
        /// </summary>
        public static IServiceLocator ServiceLocator { get; set; }

        /// <summary>
        ///     Clears the callbacks.
        /// </summary>
        public static void ClearCallbacks()
        {
            actions = null;
        }

        /// <summary>
        ///     Raises the specified event and calls appropriate event handlers and registered callbacks.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="args">The arguments to pass on to the event handlers and callbacks.</param>
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (ServiceLocator != null)
            {
                foreach (var handler in ServiceLocator.GetAllInstances<IHandles<T>>())
                {
                    handler.Handle(args);
                }
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                       ((Action<T>)action)(args);
                    }
                }
            }
        }

        /// <summary>
        ///     Registers the specified callback for events of the specified type.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="callback">The callback.</param>
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }
            actions.Add(callback);
        }
    }
}