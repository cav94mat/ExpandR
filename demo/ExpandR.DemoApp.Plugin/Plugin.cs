using System;
using System.Collections.Generic;
using System.Text;
using cav94mat.ExpandR;
using ExpandR.Demo;
using ExpandR.DemoAPI;
using ExpandR.DemoApp.Plugin;

[assembly: Entrypoint(typeof(Plugin))]

namespace ExpandR.DemoApp.Plugin
{
    public class Plugin : IEntrypoint
    {
        public void Setup(IServiceCollectionExtender services)
        {
            services.Add<IHelpPrinter, FancyHelpPrinter>();
            services.Add<ICommand, ConfCommand>();
        }
    }
}
