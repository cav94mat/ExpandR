namespace cav94mat.ExpandR
{
    /// <summary>
    /// Represents the result of the registration of a service implementation.
    /// </summary>
    public enum ServiceExtensionResult
    {
        /// <summary>
        /// The service type was not exposed to ExpandR by the host, hence it cannot be implemented.
        /// </summary>
        Undefined = -1,
        /// <summary>
        /// The service type was already implemented either by the host or another plugin, and multiple implementations are not supported.
        /// </summary>
        AlredyImplemented = -2,
        /// <summary>
        /// The service implementation has been registered successfully.
        /// </summary>
        Implemented = 0,
        /// <summary>
        /// The service implementation has been added successfully.
        /// </summary>
        Added = 1
    }
}
