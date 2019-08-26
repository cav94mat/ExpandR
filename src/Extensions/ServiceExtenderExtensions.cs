using System;
using cav94mat.ExpandR.Host;

namespace cav94mat.ExpandR
{
    public static class ServiceExtenderExtensions
    {        
        public static bool TryAdd<TService, TImplementation>(this IServiceCollectionExtender services, out ServiceExtensionResult result) where TImplementation : class, TService
            => services.TryAdd(typeof(TService), ServiceImplementation.FromType<TImplementation>(), out result);
        public static bool TryAdd<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory, out ServiceExtensionResult result)
            => services.TryAdd(typeof(TService), ServiceImplementation.FromFactory(factory), out result);

        public static bool TryAdd<TService, TImplementation>(this IServiceCollectionExtender services) where TImplementation : class, TService
            => services.TryAdd<TService, TImplementation>(out _);
        public static bool TryAdd<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory)
            => services.TryAdd<TService>(factory, out _);
        
        public static void Add<TService, TImplementation>(this IServiceCollectionExtender services) where TImplementation : class, TService
            => services.Add(typeof(TService), ServiceImplementation.FromType<TImplementation>());
        public static void Add<TService>(this IServiceCollectionExtender services, Func<IServiceProvider, TService> factory)
            => services.Add(typeof(TService), ServiceImplementation.FromFactory(factory));        
    }
}
