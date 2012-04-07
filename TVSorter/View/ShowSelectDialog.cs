// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSelectDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Dialog for selecting a show.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// Dialog for selecting a show.
    /// </summary>
    public partial class ShowSelectDialog : Form
    {
        #region Constants and Fields

        /// <summary>
        ///   The list of shows.
        /// </summary>
        private readonly List<TvShow> shows;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowSelectDialog"/> class.
        /// </summary>
        /// <param name="shows">
        /// The shows. 
        /// </param>
        public ShowSelectDialog(List<TvShow> shows)
        {
            this.shows = shows;
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets SelectedShow.
        /// </summary>
        public TvShow SelectedShow { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Close button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handles the Select button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SelectButtonClick(object sender, EventArgs e)
        {
            this.SelectedShow = (TvShow)this.showList.SelectedItem;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the dialog loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ShowSelectDialogLoad(object sender, EventArgs e)
        {
            this.showList.DisplayMember = "Name";
            this.showList.DataSource = this.shows;
        }

        #endregion
    }
}