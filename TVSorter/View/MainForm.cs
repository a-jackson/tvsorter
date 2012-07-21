// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MainForm.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The main form of the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Windows.Forms;

    using Version = TVSorter.Version;

    #endregion

    /// <summary>
    /// The main form of the program.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the load event for the form.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.Text = "TV Sorter " + Version.CurrentVersion;
        }

        #endregion
    }
}