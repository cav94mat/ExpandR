namespace cav94mat.ExpandR
{
    /// <summary>
    /// This interface is to be inherited, or implemented by ExpandR compatible entry-point classes.
    /// <para>  
    /// To specify the entry-point of a plugin, please decorate the plugin assembly with <see cref="EntrypointAttribute"/> (or a derivative of it).
    /// </para>
    /// </summary>    
    public interface IEntrypoint
    {
        /// <summary>
        /// This method is called whenever the plugin assembly is loaded.
        /// </summary>
        /// <param name="services">A service registration interface to be exposed to the plugin.</param>
        void Setup(IServiceCollectionExtender services);
    }
}
