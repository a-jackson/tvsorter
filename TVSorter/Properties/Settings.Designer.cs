// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Properties
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The default instance.
        /// </summary>
        private static readonly Settings DefaultInstance = (Settings)Synchronized(new Settings());

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Default.
        /// </summary>
        public static Settings Default
        {
            get
            {
                return DefaultInstance;
            }
        }

        #endregion
    }
}