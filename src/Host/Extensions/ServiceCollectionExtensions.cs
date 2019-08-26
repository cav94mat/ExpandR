using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ServiceCollectionExtensions
    {
        public static void AddExpandR<T>(this IServiceCollection services, Action<IExpandR> configure = default) where T : EntrypointAttribute
        {
            var expandr = new ExpandR<T>(services);
            services.AddSingleton(expandr);
            configure?.Invoke(expandr);
            expandr.AddDefaults();
        }
        public static void AddExpandR(this IServiceCollection services, Action<IExpandR> configure = default)
            => services.AddExpandR<EntrypointAttribute>(configure);
    }
}
