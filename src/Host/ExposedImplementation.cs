using System;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Represents an implementation for an <see cref="ExposedService"/>.
    /// </summary>
    public struct ExposedImplementation
    {        
        private Func<IServiceProvider, object> Factory { get; set; }
        private Type ImplType { get; set; }
        private object Instance { get; set; }
        /// <summary>
        /// Defines a factory-backed implementation.
        /// </summary>
        /// <typeparam name="T">Type of the service to be implemented.</typeparam>
        /// <param name="factory">Factory definition.</param>
        public static ExposedImplementation FromFactory<T>(Func<IServiceProvider, T> factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            return new ExposedImplementation
            {
                Factory = services => factory(services),
                ImplType = null,
                Instance = null
            };
        }
        /// <summary>
        /// Defines a type-backed implementation.
        /// </summary>
        /// <param name="implType">Type of the implementation.</param>
        public static ExposedImplementation FromType(Type implType)
        {
            if (implType is null)
                throw new ArgumentNullException(nameof(implType));
            return new ExposedImplementation
            {
                Factory = null,
                ImplType = implType,
                Instance = null
            };
        }
        /// <summary>
        /// Defines an instance-backed implementation.
        /// </summary>
        /// <typeparam name="T">Type of the service to be implemented.</typeparam>
        /// <param name="instance">Pre-initialized <typeparamref name="T"/> instance.</param>
        public static ExposedImplementation FromInstance<T>(T instance)
        {
            return new ExposedImplementation
            {                
                Factory = null,
                ImplType = null,
                Instance = instance
            };
        }
        /// <summary>
        /// Defines a type-backed implementation.
        /// </summary>
        /// <typeparam name="TImplementation">Type of the implementation.</typeparam>
        public static ExposedImplementation FromType<TImplementation>()
            => FromType(typeof(TImplementation));
        public static implicit operator ExposedImplementation(Type implType)
            => FromType(implType);
        internal ServiceDescriptor Implement(Type serviceType, ServiceLifetime lifetime)
            => (Factory != null) ? new ServiceDescriptor(serviceType, Factory, lifetime)
            : (ImplType != null) ? new ServiceDescriptor(serviceType, ImplType, lifetime)
            : new ServiceDescriptor(serviceType, Instance);
    }
}
