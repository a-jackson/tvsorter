// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="NumberInputDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog for number entry.
// </summary>
// 
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
        ///   Initializes a new instance of the <see cref="NumberInputDialog" /> class.
        /// </summary>
        public NumberInputDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Number.
        /// </summary>
        public int Number { get; private set; }

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
            if (int.TryParse(this.inputText.Text, out number))
            {
                this.Number = number;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Not a valid number");
            }
        }

        #endregion
    }
}