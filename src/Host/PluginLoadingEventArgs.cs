namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// Arguments for the <see cref="PluginLoaderOptions.OnLoading"/> event.
    /// </summary>
    public class PluginLoadingEventArgs : PluginLoadEventArgs
    {
        /// <summary>
        /// Gets or sets a value which indicates whether the <see cref="PluginLoadEventArgs.PluginAssembly">current plugin</see> should be skipped by the plugins loader.
        /// </summary>
        public bool Skip { get; set; }
    }
}
