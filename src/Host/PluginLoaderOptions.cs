using System;
using System.Collections.Generic;
using System.Text;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Options for the plugins loader.
    /// </summary>
    public class PluginLoaderOptions
    {
        /// <summary>
        /// Occurs before a plugin assembly is loaded.
        /// </summary>
        public event EventHandler<PluginLoadingEventArgs> OnLoading;
        /// <summary>
        /// Occurs after a plugin assembly was loaded.
        /// </summary>
        public event EventHandler<PluginLoadEventArgs> OnLoaded;
        /// <summary>
        /// Occurs if an exception is raised while loading a plugin assembly.
        /// </summary>
        public event EventHandler<PluginErrorEventArgs> OnError;
        protected internal void RaiseLoading(object sender, PluginLoadingEventArgs e)
            => OnLoading?.Invoke(sender, e);
        protected internal void RaiseLoaded(object sender, PluginLoadEventArgs e)
            => OnLoaded?.Invoke(sender, e);
        protected internal void RaiseError(object sender, PluginErrorEventArgs e)
            => OnError?.Invoke(sender, e);
    }
}
