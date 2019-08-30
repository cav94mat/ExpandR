using System;
using cav94mat.ExpandR.Host;
namespace cav94mat.ExpandR
{
    /// <summary>
    /// This attribute should be applied globally to the plugin assembly, in order to specify its entrypoint.
    /// <para>
    /// Custom entry-points can be defined by deriving this type on the host side, and passing it as type-argument to the appropriate <see cref="ServiceCollectionExtensions.AddExpandR{T}"> AddExpandr&lt;T&gt; call</see>.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class EntrypointAttribute : Attribute
    {
        /// <summary>
        /// This attribute should be applied globally to the plugin assembly, in order to specify its entrypoint.
        /// </summary>
        /// <param name="type">The type of the entry-point class.</param>
        public EntrypointAttribute(Type type)
        {
            Type = type;
        }
        /// <summary>
        /// Gets the type of the plugin's entry-point class.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Tries creating an instance of the entry-point class (<see cref="Type"/>), to be safely casted to the specified interface or parent class.
        /// </summary>
        /// <typeparam name="T">Type the instance should be casted to prior returning.</typeparam>
        /// <returns>An initialized instance, or null if any error occurs.</returns>
        protected T Initialize<T>() where T: IEntrypoint
            => Type?.GetConstructor(new Type[] { })?.Invoke(new object[] { }) is T entrypoint ? entrypoint : default;
        /// <summary>
        /// Override this method to specify the plugins' entry-point initialization logic.
        /// </summary>
        /// <returns>Initialized and casted entry-point instance. If null is returned, the plugin fails to load due to invalid entry-point.</returns>
        internal protected virtual IEntrypoint OnInitialization()
            => Initialize<IEntrypoint>();
    }
}
