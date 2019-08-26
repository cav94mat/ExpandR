using ExpandR.DemoAPI;
using con = System.Console;
namespace ExpandR.Demo.Defaults
{
    public class HelpPrinter : IHelpPrinter
    {
        public void Print(ICommand command)
        {
            con.WriteLine($"* {command.GetName()} {command.Syntax}");
            con.WriteLine($"  {command.Description}\n");
        }
    }
}
