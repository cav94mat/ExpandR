using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Represents the core implementation of the ExpandR engine.
    /// </summary>
    /// <typeparam name="T">Custom entry-point attribute to be searched for in plugin assemblies.</typeparam>
    internal sealed class ExpandR<T>: IExpandR where T : EntrypointAttribute
    {
        private readonly IServiceCollection _services;
        private readonly ExposedServicesCollection _defs = new ExposedServicesCollection();
        /// <summary>
        /// Initializes a new instance of the engine.
        /// </summary>
        /// <param name="services">The host <see cref="IServiceCollection"/> where plugin services implementations are to be added.</param>
        internal ExpandR(IServiceCollection services)
        {
            _services = services;
        }
        public void Expose(Type type, ServiceLifetime lifetime, bool multiple, params ExposedImplementation[] defaultImpl)
            => _defs.Add(type, new ExposedService(lifetime, multiple, defaultImpl));
        public void LoadPlugin(Assembly assembly)
        {
            var attr = assembly.GetCustomAttribute<T>();
            if (attr is null)
                throw new EntryPointNotFoundException($"The extension assembly is not decorated with the attribute {typeof(T).FullName}.");
            var entryPoint = attr.OnInitialization();
            if (entryPoint is null)
                throw new EntryPointNotFoundException($"The extension entry-point was not properly initialized.");
            entryPoint.Setup(new ServiceCollectionExtender(_services, _defs));
        }
        internal void AddDefaults()
        {
            var ex = new ServiceCollectionExtender(_services, _defs);
            foreach(var def in _defs)
                foreach (var defaultImpl in def.Value.DefaultImplementations)
                    ex.TryAdd(def.Key, defaultImpl, out _);            
        }
    }
}