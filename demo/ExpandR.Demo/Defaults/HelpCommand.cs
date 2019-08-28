using System;
using Microsoft.Extensions.DependencyInjection;
using ExpandR.DemoAPI;

namespace ExpandR.Demo.Defaults
{
    public class HelpCommand : ICommand
    {
        public HelpCommand(IHelpPrinter helpPrinter, IServiceProvider serviceProvider)
        {
            HelpPrinter = helpPrinter;
            ServiceProvider = serviceProvider;
        }
        public string Syntax => "";

        public string Description => "Displays a help reference for supported commands.";

        public IHelpPrinter HelpPrinter { get; }
        public IServiceProvider ServiceProvider { get; }

        public int Call(string[] args)
        {
            foreach (var cmd in ServiceProvider.GetServices<ICommand>())
                HelpPrinter.Print(cmd);
            return 0;
        }
    }
}
