using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace cav94mat.ExpandR.Host
{
    public interface IExpandR
    {
        void Add(Type type, ServiceLifetime lifetime = ServiceLifetime.Transient, bool multiple = false, params ServiceImplementation[] defaultImpls);
        void Load(Assembly dll);
    }
}
