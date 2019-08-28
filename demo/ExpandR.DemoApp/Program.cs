using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using cav94mat.ExpandR.Host;
using ExpandR.Demo;
using ExpandR.Demo.Defaults;
using ExpandR.DemoAPI;
using System.IO;
using System.Linq;
using System;

namespace ExpandR.DemoApp
{
    class Program
    {
        public Program(IServiceProvider services)
        {
            Services = services;
        }
        private IServiceProvider Services { get; }        
        private static void ConfigureServices(IServiceCollection services, string[] args)
        {
            services.AddSingleton<Program>();
            services.AddExpandR(pluggableServices =>
            {
                pluggableServices.AddSingleton<IHelpPrinter, HelpPrinter>();
                pluggableServices.AddMultiTransient<ICommand>(typeof(EchoCommand), typeof(HelpCommand));

                // Search for plugins in the /plugins sub-directory
                var plugDir = new DirectoryInfo(Directory.GetCurrentDirectory()).CreateSubdirectory("plugins");
                pluggableServices.Load(plugDir);
            });
        }
        public void Run(string[] args)
        {
            var cmdName = args.FirstOrDefault()?.ToLower() ?? "help";
            var cmd = Services.GetServices<ICommand>().FirstOrDefault(cmd => cmd.GetName() == cmdName);
            if (cmd is null)
                throw new NotSupportedException($"The command '{cmdName}' is not supported. Try 'help' for information.");
            cmd.Call(args.Skip(1).ToArray());
        }
        private static void Main(string[] args)
        {
            var app = new HostBuilder();
            app.ConfigureHostConfiguration(conf => conf
                .AddEnvironmentVariables()
                .AddCommandLine(args));
            app.ConfigureLogging(services => services
                .AddDebug()
                .AddConsole()
            );
            app.ConfigureServices(services => ConfigureServices(services, args));
            using (var host = app.Build())
                host.Services.GetService<Program>().Run(args);
        }
    }
}
