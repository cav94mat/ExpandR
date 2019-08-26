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
        #region Load
        public static void Load(this IExpandR expandr, FileInfo dll)
            => expandr.Load(Assembly.LoadFrom(dll.FullName));
        public static void Load(this IExpandR expandr, DirectoryInfo dllsFrom)
        {
            foreach (var dll in dllsFrom.GetFiles("*.dll").OrderBy(dllPath => dllPath.Name))
                expandr.Load(dll);
        }
        public static void Load(this IExpandR expandr, string dllsPath)
        {
            if (Directory.Exists(dllsPath))
                expandr.Load(new DirectoryInfo(dllsPath));
            else
                expandr.Load(new FileInfo(dllsPath));
        }
        #endregion
        #region Add
        public static void Add<TService>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple = false, params ServiceImplementation[] defaultImpls)
            => expandr.Add(typeof(TService), lifetime, multiple, defaultImpls);
        public static void Add<TService>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add(typeof(TService), lifetime, multiple, ServiceImplementation.FromFactory(defaultFactory));        
        public static void Add<TService>(this IExpandR expandr, ServiceLifetime lifetime, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add<TService>(lifetime, false, defaultFactory);
        public static void Add<TService, TDefaultImpl>(this IExpandR expandr, ServiceLifetime lifetime, bool multiple = false)
            => expandr.Add(typeof(TService), lifetime, multiple, ServiceImplementation.FromType<TDefaultImpl>());

        public static void AddTransient<TService>(this IExpandR expandr, bool multiple = false, params ServiceImplementation[] defaultImpls)
            => expandr.Add<TService>(ServiceLifetime.Transient, multiple, defaultImpls);
        public static void AddTransient<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> factory)
            => expandr.Add<TService>(ServiceLifetime.Transient, multiple, factory);
        public static void AddTransient<TService>(this IExpandR expandr, Func<IServiceProvider, TService> factory, bool multiple = false)
            => expandr.AddTransient<TService>(multiple, factory);
        public static void AddTransient<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Transient, multiple);


        public static void AddScoped<TService>(this IExpandR expandr, bool multiple = false, params ServiceImplementation[] defaultImpls)
            => expandr.Add<TService>(ServiceLifetime.Scoped, multiple, defaultImpls);
        public static void AddScoped<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> factory)
            => expandr.Add<TService>(ServiceLifetime.Scoped, multiple, factory);
        public static void AddScoped<TService>(this IExpandR expandr, Func<IServiceProvider, TService> factory, bool multiple = false)
            => expandr.AddScoped<TService>(multiple, factory);
        public static void AddScoped<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Scoped, multiple);


        public static void AddSingleton<TService>(this IExpandR expandr, bool multiple = false, params ServiceImplementation[] defaultImpls)
            => expandr.Add<TService>(ServiceLifetime.Singleton, multiple, defaultImpls);
        public static void AddSingleton<TService>(this IExpandR expandr, bool multiple, Func<IServiceProvider, TService> factory)
            => expandr.Add<TService>(ServiceLifetime.Singleton, multiple, factory);
        public static void AddSingleton<TService>(this IExpandR expandr, Func<IServiceProvider, TService> factory, bool multiple = false)
            => expandr.AddSingleton<TService>(multiple, factory);
        public static void AddSingleton<TService, TDefaultImpl>(this IExpandR expandr, bool multiple = false)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Singleton, multiple);
        public static void AddSingleton<TService>(this IExpandR expandr, bool multiple, TService defaultInstance)
            => expandr.Add<TService>(ServiceLifetime.Singleton, multiple, _ => defaultInstance);
        public static void AddSingleton<TService>(this IExpandR expandr, TService defaultInstance)
            => expandr.AddSingleton<TService>(false, _ => defaultInstance);

        public static void AddMulti<TService>(this IExpandR expandr, ServiceLifetime lifetime, params ServiceImplementation[] defaultImpls)
            => expandr.Add(typeof(TService), lifetime, true, defaultImpls);
        public static void AddMulti<TService>(this IExpandR expandr, ServiceLifetime lifetime, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add<TService>(lifetime, true, defaultFactory);
        public static void AddMulti<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Singleton, true);

        public static void AddMultiTransient<TService>(this IExpandR expandr, params ServiceImplementation[] defaultImpls)
            => expandr.Add(typeof(TService), ServiceLifetime.Transient, true, defaultImpls);
        public static void AddMultiTransient<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add<TService>(ServiceLifetime.Transient, true, defaultFactory);
        public static void AddMultiTransient<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Transient, true);

        public static void AddMultiScoped<TService>(this IExpandR expandr, params ServiceImplementation[] defaultImpls)
            => expandr.Add(typeof(TService), ServiceLifetime.Scoped, true, defaultImpls);
        public static void AddMultiScoped<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add<TService>(ServiceLifetime.Scoped, true, defaultFactory);
        public static void AddMultiScoped<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Scoped, true);

        public static void AddMultiSingleton<TService>(this IExpandR expandr, params ServiceImplementation[] defaultImpls)
            => expandr.Add(typeof(TService), ServiceLifetime.Singleton, true, defaultImpls);
        public static void AddMultiSingleton<TService>(this IExpandR expandr, Func<IServiceProvider, TService> defaultFactory)
            => expandr.Add<TService>(ServiceLifetime.Singleton, true, defaultFactory);
        public static void AddMultiSingleton<TService>(this IExpandR expandr, TService defaultInstance)
            => expandr.Add<TService>(ServiceLifetime.Singleton, true, _ => defaultInstance);
        public static void AddMultiSingleton<TService, TDefaultImpl>(this IExpandR expandr)
            => expandr.Add<TService, TDefaultImpl>(ServiceLifetime.Singleton, true);
        #endregion
    }
}
