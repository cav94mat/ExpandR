using System;
using System.Collections.Generic;
using cav94mat.ExpandR.Host;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR
{
    public interface IServiceCollectionExtender: IEnumerable<ServiceDescriptor>
    {
        bool TryAdd(Type serviceType, ServiceImplementation serviceImpl, out ServiceExtensionResult result);// where TImplementation : class, TInterface;+
        void Add(Type serviceType, ServiceImplementation serviceImpl);
    }
}
