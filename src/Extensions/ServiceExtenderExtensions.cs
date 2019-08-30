using System;
using System.ComponentModel;
using cav94mat.ExpandR.Host;

namespace cav94mat.ExpandR
{
    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public static class ServiceExtenderExtensions
    {
        /// <summary>
        /// Adds a service implementation, if either no implementations for said service was registered before or if multiple registrations are allowed by the host.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <typeparam name="TImplementation">Implementation type to register.</typeparam>
        /// <param name="result">Destination variable for the operation result.</param>
        /// <returns>True if the registration was successful, false otherwise.</returns>
        public static bool TryAdd<TService, TImplementation>(this IServiceCollectionExtender services, out ServiceExtensionResult result) where TImplementation : class, TService
            => services.TryAdd(typeof(TService), ExposedImplementation.FromType<TImplementation>(), out result);
        /// <summary>
        /// Adds a service implementation, if either no implementations for said service was registered before or if multiple registrations are allowed by the host.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <param name="factory">Factory implementation for the service.</param>
        /// <param name="result">Destination variable for the operation result.</param>
        /// <returns>True if the registration was successful, false otherwise.</returns>
        public static bool TryAdd<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory, out ServiceExtensionResult result)
            => services.TryAdd(typeof(TService), ExposedImplementation.FromFactory(factory), out result);
        /// <summary>
        /// Adds a service implementation, if either no implementations for said service was registered before or if multiple registrations are allowed by the host.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <typeparam name="TImplementation">Implementation type to register.</typeparam>
        /// <returns>True if the registration was successful, false otherwise.</returns>
        public static bool TryAdd<TService, TImplementation>(this IServiceCollectionExtender services) where TImplementation : class, TService
            => services.TryAdd<TService, TImplementation>(out _);
        /// <summary>
        /// Adds a service implementation, if either no implementations for said service was registered before or if multiple registrations are allowed by the host.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <param name="factory">Factory implementation for the service.</param>
        /// <returns>True if the registration was successful, false otherwise.</returns>
        public static bool TryAdd<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory)
            => services.TryAdd<TService>(factory, out _);
        /// <summary>
        /// Adds a service implementation, raising an exception if any error occurs.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <typeparam name="TImplementation">Implementation type to register.</typeparam>
        /// <exception cref="InvalidOperationException">If the implementation cannot be registered.</exception>
        public static void Add<TService, TImplementation>(this IServiceCollectionExtender services) where TImplementation : class, TService
            => services.Add(typeof(TService), ExposedImplementation.FromType<TImplementation>());
        /// <summary>
        /// Adds a service implementation, raising an exception if any error occurs.
        /// </summary>
        /// <typeparam name="TService">Type of the service to be implemented.</typeparam>
        /// <param name="factory">Factory implementation for the service.</param>
        /// <exception cref="InvalidOperationException">If the implementation cannot be registered.</exception>
        public static void Add<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory)
            => services.Add(typeof(TService), ExposedImplementation.FromFactory(factory));        
    }
}
