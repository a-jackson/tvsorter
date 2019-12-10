// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Program.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   TVSorter's Program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using Ninject;
using TheTvdbDotNet.Ninject;
using TVSorter.View;

namespace TVSorter
{
    /// <summary>
    ///     TVSorter's Program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            IKernel kernel = new StandardKernel(
                new InterfaceModule(),
                new LibraryModule(),
                new TheTvdbDotNetModule("1c1cc44893259628eca511dfbe4ebc52"));
            CompositionRoot.SetKernel(kernel);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(kernel.Get<MainForm>());
        }
    }
}
