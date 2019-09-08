using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cav94mat.ExpandR.Host
{
    /// <summary>
    /// The exception that is thrown when an ExpandR plugin assembly fails to be loaded.
    /// </summary>
    class PluginException: Exception
    {
        /// <summary>
        /// Initializes a new instance of the exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PluginException(string message, Exception innerException = null): base(message, innerException)
        {
            
        }
    }
    /// <summary>
    /// The exception that is thrown when an ExpandR plugin assembly file fails to be loaded.
    /// </summary>
    class PluginFileException : PluginException
    {
        /// <summary>
        /// Initializes a new instance of the exception.
        /// </summary>
        /// <param name="file">The plugin file that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public PluginFileException(FileInfo file, string message, Exception innerException = null): base(message, innerException)
        {
            File = file;
        }
        /// <summary>
        /// Gets the plugin file that caused the exception.
        /// </summary>
        public FileInfo File { get; }
    }
}
