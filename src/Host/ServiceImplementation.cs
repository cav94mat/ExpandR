using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    public struct ServiceImplementation
    {        
        private Func<IServiceProvider, object> Factory { get; set; }
        private Type ImplType { get; set; }
        private object Instance { get; set; }
        public static ServiceImplementation FromFactory<T>(Func<IServiceProvider, T> factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
            return new ServiceImplementation
            {
                Factory = services => factory(services),
                ImplType = null,
                Instance = null
            };
        }
        public static ServiceImplementation FromType(Type implType)
        {
            if (implType is null)
                throw new ArgumentNullException(nameof(implType));
            return new ServiceImplementation
            {
                Factory = null,
                ImplType = implType,
                Instance = null
            };
        }
        public static ServiceImplementation FromInstance<T>(T instance)
        {
            return new ServiceImplementation
            {                
                Factory = null,
                ImplType = null,
                Instance = instance
            };
        }
        public static ServiceImplementation FromType<TImplementation>()
            => FromType(typeof(TImplementation));
        public static implicit operator ServiceImplementation(Type implType)
            => FromType(implType);
        internal ServiceDescriptor Implement(Type serviceType, ServiceLifetime lifetime)
            => (Factory != null) ? new ServiceDescriptor(serviceType, Factory, lifetime)
            : (ImplType != null) ? new ServiceDescriptor(serviceType, ImplType, lifetime)
            : new ServiceDescriptor(serviceType, Instance);
    }
}
