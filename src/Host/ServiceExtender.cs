using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    public class ServiceExtender : IServiceCollectionExtender
    {
        private readonly IServiceCollection _services;
        private readonly ServiceDefinitionsCollection _defs;
        public ServiceExtender(IServiceCollection services, ServiceDefinitionsCollection defs)
        {
            _services = services;
            _defs = defs;
        }        
        public bool TryAdd(Type serviceType, ServiceImplementation serviceImpl, out ServiceExtensionResult result)
        {
            result = ServiceExtensionResult.Implemented;
            if (!_defs.TryGetValue(serviceType, out var serviceDef))
                result = ServiceExtensionResult.Undefined;
            if (_services.Any(s => s.ServiceType == serviceType))
                result = serviceDef.AllowMultiple ? ServiceExtensionResult.Added : ServiceExtensionResult.AlredyImplemented;
            var service = serviceDef.Implement(serviceType, serviceImpl);
            if (result >= 0)
            {
                _services.Add(service);
                return true;
            }
            return false;
        }     
        public void Add(Type serviceType, ServiceImplementation serviceImpl)
        {
            if (!TryAdd(serviceType, serviceImpl, out var result))
            {
                var serviceName = serviceType.FullName;
                switch (result)
                {
                    case ServiceExtensionResult.Undefined:
                        throw new InvalidOperationException($"The type '{serviceName}' is not exposed by the host context.");
                    case ServiceExtensionResult.AlredyImplemented:
                        throw new InvalidOperationException($"The type '{serviceName}' does not support multiple implementations.");
                    default:
                        throw new InvalidOperationException($"Unexpected error ({result}) while registering '{serviceName}'.");
                }
            }
        }
        public IEnumerator<ServiceDescriptor> GetEnumerator()
            => _services.Where(sd => _defs.ContainsKey(sd.ServiceType)).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
