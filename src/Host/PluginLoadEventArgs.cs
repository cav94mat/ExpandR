using System;
using System.IO;
using System.Reflection;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Arguments for the events in <see cref="PluginLoaderOptions"/>.
    /// </summary>
    public class PluginLoadEventArgs : EventArgs
    {
        /// <summary>
        /// The plugin assembly that is being loaded.
        /// </summary>
        public Assembly PluginAssembly { get; protected internal set; }
    }
}
