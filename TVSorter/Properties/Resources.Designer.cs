// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Resources.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A strongly-typed resource class, for looking up localized strings, etc.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Properties
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    #endregion

    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        #region Constants and Fields

        /// <summary>
        ///   The resource culture.
        /// </summary>
        private static CultureInfo resourceCulture;

        /// <summary>
        ///   The resource man.
        /// </summary>
        private static ResourceManager resourceMan;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }

            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Gets the cached ResourceManager instance used by this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(resourceMan, null))
                {
                    var temp = new ResourceManager("TVSorter.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }

                return resourceMan;
            }
        }

        /// <summary>
        ///   Gets _lock.
        /// </summary>
        internal static Bitmap _lock
        {
            get
            {
                object obj = ResourceManager.GetObject("_lock", resourceCulture);
                return (Bitmap)obj;
            }
        }

        #endregion
    }
}