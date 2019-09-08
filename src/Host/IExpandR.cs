using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Represents the ExpandR engine.
    /// </summary>
    public interface IExpandR
    {
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <param name="type">The service (interface) type.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpl">Default implementation(s) for the service.</param>
        void Expose(Type type, ServiceLifetime lifetime = ServiceLifetime.Transient, bool multiple = false, params ExposedImplementation[] defaultImpls);
        /// <summary>
        /// Searches for a compatible entry-point in the specified assembly, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="assembly">The plugin assembly.</param>
        /// <param name="options">Options for the plugins loader.</param>
        void LoadPlugin(Assembly dll, PluginLoaderOptions options = default);
    }
}
