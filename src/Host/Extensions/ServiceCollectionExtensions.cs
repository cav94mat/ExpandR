using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Allows the extension of this <see cref="IServiceCollection"/> instance via plugins.
        /// </summary>
        /// <typeparam name="T">Type of the custom entry-point attribute to be searched for in plugin assemblies.</typeparam>
        /// <param name="services"></param>
        /// <param name="configure">Configuration script for the ExpandR engine.</param>
        public static void AddExpandR<T>(this IServiceCollection services, Action<IExpandR> configure = default) where T : EntrypointAttribute
        {
            var expandr = new ExpandR<T>(services);
            services.AddSingleton(expandr);
            configure?.Invoke(expandr);
            expandr.AddDefaults();
        }
        /// <summary>
        /// Allows the extension of this <see cref="IServiceCollection"/> instance via plugins.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Configuration script for the ExpandR engine.</param>
        public static void AddExpandR(this IServiceCollection services, Action<IExpandR> configure = default)
            => services.AddExpandR<EntrypointAttribute>(configure);
    }
}
