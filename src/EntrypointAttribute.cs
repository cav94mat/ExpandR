﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text;

namespace cav94mat.ExpandR
{
    /// <summary>
    /// This attribute should be applied globally to the plugin assembly, in order to specify its entrypoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class EntrypointAttribute : Attribute
    {
        /// <summary>
        /// This attribute should be applied globally to the plugin assembly, in order to specify its entrypoint.
        /// </summary>
        /// <param name="type">The type of the entry-point class.</param>
        public EntrypointAttribute(Type type)
        {
            Type = type;
        }
        /// <summary>
        /// Gets the type of the plugin's entry-point class.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Tries creating an instance of the entry-point class (<see cref="Type"/>), to be safely casted to the specified interface or parent class.
        /// </summary>
        /// <typeparam name="T">Type the instance should be casted to prior returning.</typeparam>
        /// <returns>An initialized instance, or null if any error occurs.</returns>
        protected T Initialize<T>() where T: IEntrypoint
            => Type?.GetConstructor(new Type[] { })?.Invoke(new object[] { }) is T entrypoint ? entrypoint : default;
        /// <summary>
        /// Override this method to specify the plugins' entry-point initialization logic.
        /// </summary>
        /// <returns>Initialized and casted entry-point instance. If null is returned, the plugin fails to load due to invalid entry-point.</returns>
        internal protected virtual IEntrypoint OnInitialization()
            => Initialize<IEntrypoint>();
    }
}