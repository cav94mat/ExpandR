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
        private const string DefaultDirPattern = "*.dll";
        /// <summary>
        /// Searches for a compatible entry-point in the specified file, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="file">The plugin assembly binary, to be loaded via reflection.</param>      
        /// <param name="options">Options for the plugins loader.</param>
        public static void LoadPlugin(this IExpandR expandr, FileInfo file, PluginLoaderOptions options = default)
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(file.FullName);
            }
            catch (Exception ex)
            {
                options?.RaiseError(expandr, new PluginErrorEventArgs() { Error = new PluginFileException(file, $"Invalid assembly file: {ex.Message}", ex) });
                return;
            }
            expandr.LoadPlugin(asm, options);
        }
        /// <summary>
        /// Searches for a compatible entry-point in every compatible plugin file found in the specified directory, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="directory">The plugin assemblies directory, that are loaded via reflection after being sorted alphabetically.</param>
        /// <param name="options">Options for the plugins loader.</param>
        /// <param name="pattern">Pattern to search for in the name of the files within the directory.</param> 
        public static void LoadPlugins(this IExpandR expandr, DirectoryInfo directory, PluginLoaderOptions options = default, string pattern = DefaultDirPattern)
        {
            Directory.CreateDirectory(directory.FullName);
            foreach (var file in directory.GetFiles(pattern).OrderBy(dllPath => dllPath.Name))
                expandr.LoadPlugin(file, options);
        }
        /// <summary>
        /// Searches for a compatible entry-point in every compatible plugin file found in the specified directory, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="directory">The plugin assemblies directory, that are loaded via reflection after being sorted alphabetically.</param>
        /// <param name="pattern">Pattern to search for in the name of the files within the directory.</param> 
        public static void LoadPlugins(this IExpandR expandr, DirectoryInfo directory, string pattern)
            => expandr.LoadPlugins(directory, default, pattern);
        /// <summary>
        /// Searches for a compatible entry-point in every compatible plugin file found in the specified directory, and call its <see cref="IEntrypoint.Setup">Setup</see> method.
        /// </summary>
        /// <param name="path">Either the path of a directory or an existing file.</param>
        /// <param name="options">Options for the plugins loader.</param>
        /// <param name="pattern">Pattern to search for in the name of the files within the directory, if a directory path was supplied.</param> 
        public static void LoadPlugins(this IExpandR expandr, string path, PluginLoaderOptions options = default, string pattern = DefaultDirPattern)
        {
            if (File.Exists(path))
                expandr.LoadPlugin(new FileInfo(Path.GetDirectoryName(path)), options);
            else
                expandr.LoadPlugins(new DirectoryInfo(path), options, pattern);
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
