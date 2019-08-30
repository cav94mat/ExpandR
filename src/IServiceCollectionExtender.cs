using System;
using System.Collections.Generic;
using cav94mat.ExpandR.Host;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR
{
    /// <summary>
    /// An interface for plugin entry-points in order to register its service implementations.
    /// </summary>
    public interface IServiceCollectionExtender: IEnumerable<ServiceDescriptor>
    {
        /// <summary>
        /// Adds a service implementation, if either no implementations for said service was registered before or if multiple registrations are allowed by the host.
        /// </summary>
        /// <param name="serviceType">Type of the service to be implemented.</param>
        /// <param name="serviceImpl">Implementation type to register</param>
        /// <param name="result">Destination variable for the operation result.</param>
        /// <returns>True if the registration was successful, false otherwise.</returns>
        bool TryAdd(Type serviceType, ExposedImplementation serviceImpl, out ServiceExtensionResult result); // where TImplementation : class, TInterface;
        /// <summary>
        /// Adds a service implementation, raising an exception if any error occurs.
        /// </summary>
        /// <param name="serviceType">Type of the service to be implemented.</param>
        /// <param name="serviceImpl">Implementation type to register</param>
        /// <exception cref="InvalidOperationException">If the implementation cannot be registered.</exception>
        void Add(Type serviceType, ExposedImplementation serviceImpl);
    }
}
