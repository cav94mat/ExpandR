using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ExpandR.DemoAPI;

namespace ExpandR.Demo
{
    public static class CommandExtensions
    {
        public static string GetName(this ICommand command)
        {
            // PrintHelpCommand => "printHelp"
            var cmdName = command.GetType().Name;
            if (cmdName.EndsWith("Command"))
                cmdName = cmdName.Substring(0, cmdName.Length - 7);
            return $"{cmdName.Substring(0, 1).ToLowerInvariant()}{cmdName.Substring(1)}";
        }
    }
}
