// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Program.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   TVSorter's Program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Windows.Forms;

    using TVSorter.View;

    #endregion

    /// <summary>
    /// TVSorter's Program.
    /// </summary>
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        #endregion
    }
}