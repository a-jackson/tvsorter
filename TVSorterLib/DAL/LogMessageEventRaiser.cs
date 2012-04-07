// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="LogMessageEventRaiser.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Defines the signature of the log message event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
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
    public delegate void LogMessageEventRaiser(string message, params object[] args);
}