using System;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Represents the definition of a service exposed to plugins.
    /// </summary>
    public struct ExposedService
    {
        /// <summary>
        /// Initializes an exposed service definition.
        /// </summary>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="allowMultiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpls">Default implementation(s) for the service.</param>
        public ExposedService(ServiceLifetime lifetime, bool allowMultiple, ExposedImplementation[] defaultImpls)
        {
            Lifetime = lifetime;
            AllowMultiple = allowMultiple;
            DefaultImplementations = defaultImpls ?? new ExposedImplementation[0];
            if (!AllowMultiple && DefaultImplementations.Length > 1)
                throw new ArgumentException("The service supports at most one implementation.", nameof(defaultImpls));
        }
        /// <summary>
        /// Gets the lifetime of the service.
        /// </summary>
        public ServiceLifetime Lifetime { get; }
        /// <summary>
        /// Gets a value which indicates whether the service can have multiple implementations, or at most one.
        /// </summary>
        public bool AllowMultiple { get; }
        /// <summary>
        /// Gets the default implementation(s) for the service.
        /// </summary>
        public ExposedImplementation[] DefaultImplementations { get; }
        internal ServiceDescriptor Implement(Type type, ExposedImplementation impl)
            => impl.Implement(type, Lifetime);
    }
}
