// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="NumberInputDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog for number entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;

namespace TVSorter.View
{
    /// <summary>
    ///     A dialog for number entry.
    /// </summary>
    public partial class NumberInputDialog : Form
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="NumberInputDialog" /> class.
        /// </summary>
        public NumberInputDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Gets the episode number.
        /// </summary>
        public int EpisodeNumber { get; private set; }

        /// <summary>
        ///     Gets the season number.
        /// </summary>
        public int SeasonNumber { get; private set; }

        /// <summary>
        ///     Handles the Cancel button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        ///     Handles the OK button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OkButtonClick(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(seasonNumber.Text, out number))
            {
                SeasonNumber = number;
            }
            else
            {
                MessageBox.Show("Season number is not a valid number.");
                return;
            }

            if (int.TryParse(episodeNumber.Text, out number))
            {
                EpisodeNumber = number;
            }
            else
            {
                MessageBox.Show("Episode number is not a valid number.");
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
