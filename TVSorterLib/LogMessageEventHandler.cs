// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="LogMessageEventHandler.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Defines the signature of the log message event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter
{
    /// <summary>
    /// Defines the signature of the log message event.
    /// </summary>
    /// <param name="message">
    /// The message. 
    /// </param>
    /// <param name="args">
    /// The args. 
    /// </param>
    public delegate void LogMessageEventHandler(string message, params object[] args);
}