using System;
using ExpandR.Demo;
using ExpandR.DemoAPI;
using con = System.Console;
namespace ExpandR.DemoApp.Plugin
{
    class FancyHelpPrinter : IHelpPrinter
    {
        public void Print(ICommand command)
        {            
            con.ForegroundColor = ConsoleColor.White;
            con.Write("* ");
            con.ForegroundColor = ConsoleColor.Cyan;
            con.Write($"{command.GetName()} ");
            con.ForegroundColor = ConsoleColor.Yellow;            
            con.WriteLine(command.Syntax);
            con.ForegroundColor = ConsoleColor.Gray;
            con.WriteLine($"  {command.Description}\n");
            con.ResetColor();
        }
    }
}
