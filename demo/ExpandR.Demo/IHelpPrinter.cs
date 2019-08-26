using System;
using System.Collections.Generic;
using System.Text;
using ExpandR.DemoAPI;

namespace ExpandR.Demo
{
    public interface IHelpPrinter
    {
        void Print(ICommand command);
    }
}
