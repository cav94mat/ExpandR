using System;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    public struct ServiceDefinition
    {
        public ServiceDefinition(ServiceLifetime lifetime, bool allowMultiple, ServiceImplementation[] defaultImpls)
        {
            Lifetime = lifetime;
            AllowMultiple = allowMultiple;
            DefaultImplementations = defaultImpls ?? new ServiceImplementation[0];
            if (!AllowMultiple && DefaultImplementations.Length > 1)
                throw new ArgumentException("Multiple default implementations defined for a service that only supports one.", nameof(defaultImpls));
        }
        public ServiceLifetime Lifetime { get; }
        public bool AllowMultiple { get; }
        public ServiceImplementation[] DefaultImplementations { get; }
        internal ServiceDescriptor Implement(Type type, ServiceImplementation impl)
            => impl.Implement(type, Lifetime);
    }
}
