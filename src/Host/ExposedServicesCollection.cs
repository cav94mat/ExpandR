using System;
using System.Collections.Generic;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Represent a service-type indexed collection of <see cref="ExposedService"/>s.
    /// </summary>
    public class ExposedServicesCollection: Dictionary<Type, ExposedService>
    {

    }
}
