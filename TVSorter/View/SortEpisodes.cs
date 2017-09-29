// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SortEpisodes.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The sort episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TVSorter.Controller;
using TVSorter.Files;

namespace TVSorter.View
{
    /// <summary>
    ///     The sort episodes tab.
    /// </summary>
    public partial class SortEpisodes : UserControl, IView
    {
        private readonly IFileResultManager fileResultManager;

        /// <summary>
        ///     The controller.
        /// </summary>
        private SortEpisodesController controller;

        /// <summary>
        ///     Initialises a new instance of the <see cref="SortEpisodes" /> class.
        /// </summary>
        public SortEpisodes()
        {
            InitializeComponent();
            setEpisodeButton.Enabled = false;
            setShowButton.Enabled = false;

            fileResultManager = CompositionRoot.Get<IFileResultManager>();
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
            var dialog = new ProgressDialog(task) { Text = taskName };
            dialog.ShowDialog(this);
        }

        /// <summary>
        ///     Handles a property changing on the controller.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ControllerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ControllerOnPropertyChanged(sender, e)));
            }
            else
            {
                switch (e.PropertyName)
                {
                    case "SubDirectories":
                        subDirectoryFilter.DataSource = controller.SubDirectories;
                        break;
                    case "Results":
                        ProcessResults();
                        break;
                }
            }
        }

        /// <summary>
        ///     Handles the Copy button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void CopyButtonClick(object sender, EventArgs e)
        {
            controller.CopyEpisodes();
        }

        /// <summary>
        ///     Handles the Deselect all button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void DeselectAllButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in resultsList.Items)
            {
                item.Checked = false;
            }
        }

        /// <summary>
        ///     Handles the Move button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void MoveButtonClick(object sender, EventArgs e)
        {
            controller.MoveEpisodes();
        }

        /// <summary>
        ///     Processes the results of a scan
        /// </summary>
        private void ProcessResults()
        {
            resultsList.Items.Clear();
            resultsList.Items.AddRange(controller.Results.Select(x => x.GetListViewItem(fileResultManager)).ToArray());
        }

        /// <summary>
        ///     Resets the data for the specified items.
        /// </summary>
        private void ResetListItems()
        {
            for (var i = 0; i < controller.Results.Count; i++)
            {
                resultsList.Items[i] = controller.Results[i].GetListViewItem(fileResultManager);
            }
        }

        /// <summary>
        ///     Handles an item being checked or unchecked in the results list.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ResultsListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            controller.Results[e.Item.Index].Checked = e.Item.Checked;

            if (controller.Results.Count(p => p.Checked) > 0)
            {
                setShowButton.Enabled = true;
            }

            if (controller.Results.Count(p => p.Checked) == 1)
            {
                setEpisodeButton.Enabled = true;
            }

            if (controller.Results.Count(p => p.Checked) > 1)
            {
                setEpisodeButton.Enabled = false;
            }
            else if (controller.Results.Count(p => p.Checked) == 0)
            {
                setEpisodeButton.Enabled = false;
                setShowButton.Enabled = false;
            }
        }

        /// <summary>
        ///     Handles the Scan button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ScanButtonClick(object sender, EventArgs e)
        {
            controller.ScanEpisodes((string)subDirectoryFilter.SelectedItem);
        }

        /// <summary>
        ///     Handles the Select All button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SelectAllButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in resultsList.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        ///     Handles the Set Episode button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SetEpisodeButtonClick(object sender, EventArgs e)
        {
            var dialog = new NumberInputDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                controller.SetEpisode(dialog.SeasonNumber, dialog.EpisodeNumber);
                ResetListItems();
            }
        }

        /// <summary>
        ///     Handles the Set show button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SetShowButtonClick(object sender, EventArgs e)
        {
            var dialog = new ShowSelectDialog(controller.Shows);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                controller.SetShow(dialog.SelectedShow);

                // Update the listitems with the altered results
                ResetListItems();
            }
        }

        /// <summary>
        ///     Handles the tab loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SortEpisodesLoad(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                controller = CompositionRoot.Get<SortEpisodesController>();
                controller.PropertyChanged += ControllerOnPropertyChanged;
                controller.Initialise(this);
            }
        }
    }
}
