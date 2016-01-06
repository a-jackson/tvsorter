// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Dialog for searching for a show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using TVSorter.Controller;
    using TVSorter.Model;

    #endregion

    /// <summary>
    /// Dialog for searching for a show.
    /// </summary>
    public partial class ShowSearchDialog : Form, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private readonly ShowSearchController controller;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ShowSearchDialog"/> class.
        /// </summary>
        /// <param name="controller">
        /// The controller. 
        /// </param>
        public ShowSearchDialog(ShowSearchController controller)
        {
            this.controller = controller;
            this.InitializeComponent();

            controller.PropertyChanged += this.PropertyChanged;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Starts the progress indication for the specified Project Task.
        /// </summary>
        /// <param name="task">
        /// The task. 
        /// </param>
        /// <param name="taskName">
        /// The task name. 
        /// </param>
        public void StartTaskProgress(IProgressTask task, string taskName)
        {
            // Not needed in this dialog.
        }

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
            this.Close();
        }

        /// <summary>
        /// Handles the list of results being double clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ListResultsDoubleClick(object sender, EventArgs e)
        {
            if (this.listResults.SelectedIndices.Count != 1)
            {
                return;
            }

            this.controller.Select(this.listResults.SelectedIndices[0]);
            this.Close();
        }

        /// <summary>
        /// Handles a property of the controller changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title":
                    this.Text = this.controller.Title;
                    break;
                case "CloseButtonText":
                    this.closeButton.Text = this.controller.CloseButtonText;
                    break;
                case "SearchResults":
                    this.listResults.Items.Clear();
                    if (this.controller.SearchResults != null)
                    {
                        foreach (TvShow show in this.controller.SearchResults)
                        {
                            this.listResults.Items.Add(new ListViewItem(new[] { show.Name, show.TvdbId }));
                        }

                        if (this.controller.SearchResults.Count > 0)
                        {
                            this.listResults.Items[0].Selected = true;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the Search button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SearchButtonClick(object sender, EventArgs e)
        {
            this.controller.Search(this.nameText.Text);
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
            if (this.listResults.SelectedIndices.Count == 1)
            {
                this.controller.Select(this.listResults.SelectedIndices[0]);
                this.Close();
            }
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
        private void ShowSearchDialogLoad(object sender, EventArgs e)
        {
            this.controller.Initialise(this);
        }

        #endregion
    }
}