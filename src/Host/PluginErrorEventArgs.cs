using System;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Arguments for the <see cref="PluginLoaderOptions.OnError"/> event.
    /// </summary>
    public class PluginErrorEventArgs : PluginLoadEventArgs
    {
        /// <summary>
        /// Instance of the exception occurred.
        /// </summary>        
        public Exception Error { get; protected internal set; }
        /// <summary>
        /// Propagates the <see cref="Error">exception</see> to the host context.
        /// </summary>
        public void PropagateError() => throw Error;
    }
}
