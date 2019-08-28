namespace cav94mat.ExpandR
{
    /// <summary>
    /// This interface is to be inherited, or implemented by ExpandR compatible entry-point classes.
    /// </summary>
    /// <remarks>
    /// To specify the entry-point of a plugin, please apply <see cref="EntrypointAttribute"/> (or a derivative) globally to the assembly.
    /// </remarks>
    public interface IEntrypoint
    {
        /// <summary>
        /// This method is called whenever the assembly is loaded.
        /// </summary>
        /// <param name="services">A service registration interface to be exposed to the plugin.</param>
        void Setup(IServiceCollectionExtender services);
    }
}
