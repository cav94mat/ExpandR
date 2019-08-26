using System;
using System.Collections.Generic;
using System.Text;
using ExpandR.DemoAPI;
using Microsoft.Extensions.Configuration;
using con = System.Console;
namespace ExpandR.Demo.Defaults
{
    public class EchoCommand : ICommand
    {
        private string _ifs;
        public EchoCommand(IConfiguration conf)
        {
            _ifs = conf.GetValue("IFS", "\n");
        }
        public string Syntax
            => "<<args>>";

        public string Description
            => "Prints all the arguments in succession, each followed by the value of $IFS.";

        public int Call(string[] args)
        {
            foreach(var arg in args)
            {
                con.Write(arg);
                con.Write(_ifs);
            }
            return 0;
        }
    }
}
