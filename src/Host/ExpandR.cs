using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    public class ExpandR<T>: IExpandR where T : EntrypointAttribute
    {
        private readonly IServiceCollection _services;
        private readonly ServiceDefinitionsCollection _defs = new ServiceDefinitionsCollection();
        public ExpandR(IServiceCollection services)
        {
            _services = services;
        }

        public void Add(Type type, ServiceLifetime lifetime, bool multiple, params ServiceImplementation[] defaultImpl)
            => _defs.Add(type, new ServiceDefinition(lifetime, multiple, defaultImpl));

        public void Load(Assembly assembly)
        {
            var attr = assembly.GetCustomAttribute<T>();
            if (attr is null)
                throw new EntryPointNotFoundException($"The extension assembly is not decorated with the attribute {typeof(T).FullName}.");
            var entryPoint = attr.OnInitialization();
            if (entryPoint is null)
                throw new EntryPointNotFoundException($"The extension entry-point was not properly initialized.");
            entryPoint.Setup(new ServiceExtender(_services, _defs));
        }
        public void AddDefaults()
        {
            var ex = new ServiceExtender(_services, _defs);
            foreach(var def in _defs)
                foreach (var defaultImpl in def.Value.DefaultImplementations)
                    ex.TryAdd(def.Key, defaultImpl, out _);            
        }
    }
}