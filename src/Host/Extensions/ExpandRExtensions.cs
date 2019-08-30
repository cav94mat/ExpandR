using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExpandRExtensions
    {
        #region LoadPlugin(s)
        /// <summary>
        /// Searches for a compatible entry-point in the specified file, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="dll">The plugin assembly binary, to be loaded via reflection.</param>
        /// <exception cref="EntryPointNotFoundException">If the specified assembly doesn't have a suitable entry-point.</exception>        
        public static void LoadPlugin(this IExpandR expandr, FileInfo dll)
            => expandr.LoadPlugin(Assembly.LoadFrom(dll.FullName));
        /// <summary>
        /// Searches for a compatible entry-point in every <c>.dll</c> file found in the specified directory, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="dllsFrom">The plugin assemblies directory, that are loaded via reflection after being sorted alphabetically.</param>
        /// <exception cref="EntryPointNotFoundException">If one of the assemblies doesn't have a suitable entry-point.</exception>        
        public static void LoadPlugins(this IExpandR expandr, DirectoryInfo dllsFrom)
        {
            foreach (var dll in dllsFrom.GetFiles("*.dll").OrderBy(dllPath => dllPath.Name))
                expandr.LoadPlugin(dll);
        }
        /// <summary>
        /// Searches for a compatible entry-point in every .dll file in the specified directory, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="dllsFrom">The plugin assemblies directory, that are loaded via reflection after being sorted alphabetically.</param>
        /// <exception cref="EntryPointNotFoundException">If any of the assemblies doesn't have a suitable entry-point.</exception> 
        public static void LoadPlugins(this IExpandR expandr, string dllsPath)
        {
            if (Directory.Exists(dllsPath))
                expandr.LoadPlugins(new DirectoryInfo(dllsPath));
            else
                expandr.LoadPlugin(new FileInfo(dllsPath));
        }
        #endregion
        #region Expose
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpls">Default implementation(s) for the service.</param>
        public static void Expose<TService>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple = false, params ExposedImplementation[] defaultImpls)
            => expandr.Expose(typeof(TService), lifetime, multiple, defaultImpls);
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void Expose<TService>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose(typeof(TService), lifetime, multiple, ExposedImplementation.FromFactory(defaultFactory));
        /// <summary>
        /// Exposes a service type to ExpandR, so that one plugin can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void Expose<TService>(this IExpandR expandr, ServiceLifetime lifetime, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(lifetime, false, defaultFactory);
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void Expose<TService, TDefaultImpl>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple = false)
            => expandr.Expose(typeof(TService), lifetime, multiple, ExposedImplementation.FromType<TDefaultImpl>());
        
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpls">Default implementation(s) for the service.</param>
        public static void ExposeTransient<TService>(this IExpandR expandr, bool multiple = false, params ExposedImplementation[] defaultImpls)
            => expandr.Expose<TService>(ServiceLifetime.Transient, multiple, defaultImpls);
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeTransient<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Transient, multiple, defaultFactory);
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>        
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeTransient<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory, bool multiple = false)
            => expandr.ExposeTransient<TService>(multiple, defaultFactory);
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeTransient<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Transient, multiple);

        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpls">Default implementation(s) for the service.</param>
        public static void ExposeScoped<TService>(this IExpandR expandr, bool multiple = false, params ExposedImplementation[] defaultImpls)
            => expandr.Expose<TService>(ServiceLifetime.Scoped, multiple, defaultImpls);
        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeScoped<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Scoped, multiple, defaultFactory);
        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>        
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeScoped<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory, bool multiple = false)
            => expandr.ExposeScoped<TService>(multiple, defaultFactory);
        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeScoped<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Scoped, multiple);

        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultImpls">Default implementation(s) for the service.</param>
        public static void ExposeSingleton<TService>(this IExpandR expandr, bool multiple = false, params ExposedImplementation[] defaultImpls)
            => expandr.Expose<TService>(ServiceLifetime.Singleton, multiple, defaultImpls);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeSingleton<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Singleton, multiple, defaultFactory);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>        
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeSingleton<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory, bool multiple = false)
            => expandr.ExposeSingleton<TService>(multiple, defaultFactory);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        public static void ExposeSingleton<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Singleton, multiple);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>            
        /// <param name="multiple">Whether the service can have multiple implementations, or at most one.</param>
        /// <param name="defaultInstance">Default instance implementation for the service.</param>    
        public static void ExposeSingleton<TService>(this IExpandR expandr, bool multiple, TService defaultInstance)
            => expandr.Expose<TService>(ServiceLifetime.Singleton, multiple, _ => defaultInstance);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins can implement it.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultInstance">Default instance implementation for the service.</param>    
        public static void ExposeSingleton<TService>(this IExpandR expandr, TService defaultInstance)
            => expandr.ExposeSingleton<TService>(false, _ => defaultInstance);

        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>       
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="defaultImpls">Default implementations for the service.</param>    
        public static void ExposeMulti<TService>(this IExpandR expandr, ServiceLifetime lifetime, params ExposedImplementation[] defaultImpls)
            => expandr.Expose(typeof(TService), lifetime, true, defaultImpls);
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>       
        /// <param name="lifetime">The lifetime of the service.</param>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeMulti<TService>(this IExpandR expandr, ServiceLifetime lifetime, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(lifetime, true, defaultFactory);
        /// <summary>
        /// Exposes a service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        public static void ExposeMulti<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Singleton, true);

        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultImpls">Default implementations for the service.</param>
        public static void ExposeMultiTransient<TService>(this IExpandR expandr, params ExposedImplementation[] defaultImpls)
            => expandr.Expose(typeof(TService), ServiceLifetime.Transient, true, defaultImpls);
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeMultiTransient<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Transient, true, defaultFactory);
        /// <summary>
        /// Exposes a transient service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>   
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        public static void ExposeMultiTransient<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Transient, true);

        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultImpls">Default implementations for the service.</param>
        public static void ExposeMultiScoped<TService>(this IExpandR expandr, params ExposedImplementation[] defaultImpls)
            => expandr.Expose(typeof(TService), ServiceLifetime.Scoped, true, defaultImpls);
        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultFactory">Default factory implementation for the service.</param>
        public static void ExposeMultiScoped<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Scoped, true, defaultFactory);
        /// <summary>
        /// Exposes a scoped service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>   
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        public static void ExposeMultiScoped<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Scoped, true);

        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>
        /// <param name="defaultImpls">Default implementations for the service.</param>
        public static void ExposeMultiSingleton<TService>(this IExpandR expandr, params ExposedImplementation[] defaultImpls)
            => expandr.Expose(typeof(TService), ServiceLifetime.Singleton, true, defaultImpls);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>   
        /// <param name="defaultImpls">Default implementations for the service.</param>
        public static void ExposeMultiSingleton<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Expose<TService>(ServiceLifetime.Singleton, true, defaultFactory);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>       
        /// <param name="defaultInstance">Default instance implementations for the service.</param>
        public static void ExposeMultiSingleton<TService>(this IExpandR expandr, TService defaultInstance)
            => expandr.Expose<TService>(ServiceLifetime.Singleton, true, _ => defaultInstance);
        /// <summary>
        /// Exposes a singleton service type to ExpandR, so that plugins may provide multiple implementations.
        /// </summary>
        /// <typeparam name="TService">The service (interface) type.</typeparam>   
        /// <typeparam name="TDefaultImpl">The default implementation type.</typeparam>
        public static void ExposeMultiSingleton<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Expose<TService, TDefaultImpl>(ServiceLifetime.Singleton, true);
        #endregion
    }
}
