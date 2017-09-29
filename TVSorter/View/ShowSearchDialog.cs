// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Dialog for searching for a show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows.Forms;
using TVSorter.Controller;

namespace TVSorter.View
{
    /// <summary>
    ///     Dialog for searching for a show.
    /// </summary>
    public partial class ShowSearchDialog : Form, IView
    {
        /// <summary>
        ///     The controller.
        /// </summary>
        private readonly ShowSearchController controller;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ShowSearchDialog" /> class.
        /// </summary>
        /// <param name="controller">
        ///     The controller.
        /// </param>
        public ShowSearchDialog(ShowSearchController controller)
        {
            this.controller = controller;
            InitializeComponent();

            controller.PropertyChanged += PropertyChanged;
        }

        /// <summary>
        ///     Starts the progress indication for the specified Project Task.
        /// </summary>
        /// <param name="task">
        ///     The task.
        /// </param>
        /// <param name="taskName">
        ///     The task name.
        /// </param>
        public void StartTaskProgress(IProgressTask task, string taskName)
        {
            // Not needed in this dialog.
        }

        /// <summary>
        ///     Handles the Close button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Handles the list of results being double clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ListResultsDoubleClick(object sender, EventArgs e)
        {
            if (listResults.SelectedIndices.Count != 1)
            {
                return;
            }

            controller.Select(listResults.SelectedIndices[0]);
            Close();
        }

        /// <summary>
        ///     Handles a property of the controller changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Title":
                    Text = controller.Title;
                    break;
                case "CloseButtonText":
                    closeButton.Text = controller.CloseButtonText;
                    break;
                case "SearchResults":
                    listResults.Items.Clear();
                    if (controller.SearchResults != null)
                    {
                        foreach (var show in controller.SearchResults)
                        {
                            listResults.Items.Add(new ListViewItem(new[] { show.Name, show.TvdbId.ToString() }));
                        }

                        if (controller.SearchResults.Count > 0)
                        {
                            listResults.Items[0].Selected = true;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        ///     Handles the Search button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SearchButtonClick(object sender, EventArgs e)
        {
            controller.Search(nameText.Text);
        }

        /// <summary>
        ///     Handles the Select button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SelectButtonClick(object sender, EventArgs e)
        {
            if (listResults.SelectedIndices.Count == 1)
            {
                controller.Select(listResults.SelectedIndices[0]);
                Close();
            }
        }

        /// <summary>
        ///     Handles the dialog loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ShowSearchDialogLoad(object sender, EventArgs e)
        {
            controller.Initialise(this);
        }
    }
}
