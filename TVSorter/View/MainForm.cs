// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MainForm.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The main form of the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace TVSorter.View
{
    /// <summary>
    ///     The main form of the program.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the load event for the form.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            Text = "TV Sorter " + Version.CurrentVersion;
        }
    }
}
