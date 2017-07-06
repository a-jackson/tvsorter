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

    using Ninject;
    using TVSorter.View;
    using TheTvdbDotNet.Ninject;

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
            IKernel kernel = new StandardKernel(
                new InterfaceModule(),
                new LibraryModule(),
                new TheTvdbDotNetModule("D4DCAEBFCA5A6BC1"));
            CompositionRoot.SetKernel(kernel);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(kernel.Get<MainForm>());
        }

        #endregion
    }
}