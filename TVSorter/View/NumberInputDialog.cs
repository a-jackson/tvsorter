// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="NumberInputDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog for number entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// A dialog for number entry.
    /// </summary>
    public partial class NumberInputDialog : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initialises a new instance of the <see cref="NumberInputDialog" /> class.
        /// </summary>
        public NumberInputDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the episode number.
        /// </summary>
        public int EpisodeNumber { get; private set; }

        /// <summary>
        ///   Gets the season number.
        /// </summary>
        public int SeasonNumber { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Cancel button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handles the OK button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OkButtonClick(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(this.seasonNumber.Text, out number))
            {
                this.SeasonNumber = number;
            }
            else
            {
                MessageBox.Show("Season number is not a valid number.");
                return;
            }

            if (int.TryParse(this.episodeNumber.Text, out number))
            {
                this.EpisodeNumber = number;
            }
            else
            {
                MessageBox.Show("Episode number is not a valid number.");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}