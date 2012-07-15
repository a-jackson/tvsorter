// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SortEpisodes.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The sort episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;

    using TVSorter.Controller;

    #endregion

    /// <summary>
    /// The sort episodes tab.
    /// </summary>
    public partial class SortEpisodes : UserControl, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private readonly SortEpisodesController controller;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortEpisodes" /> class.
        /// </summary>
        public SortEpisodes()
        {
            this.InitializeComponent();

            this.controller = new SortEpisodesController();
            this.controller.PropertyChanged += this.ControllerOnPropertyChanged;
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
            var dialog = new ProgressDialog(task) { Text = taskName };
            dialog.ShowDialog(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles a property changing on the controller.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ControllerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.ControllerOnPropertyChanged(sender, e)));
            }
            else
            {
                switch (e.PropertyName)
                {
                    case "SubDirectories":
                        this.subDirectoryFilter.DataSource = this.controller.SubDirectories;
                        break;
                    case "Results":
                        this.ProcessResults();
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Copy button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void CopyButtonClick(object sender, EventArgs e)
        {
            this.controller.CopyEpisodes();
        }

        /// <summary>
        /// Handles the Deselect all button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void DeselectAllButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.resultsList.Items)
            {
                item.Checked = false;
            }
        }

        /// <summary>
        /// Handles the Move button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void MoveButtonClick(object sender, EventArgs e)
        {
            this.controller.MoveEpisodes();
        }

        /// <summary>
        /// Processes the results of a scan
        /// </summary>
        private void ProcessResults()
        {
            this.resultsList.Items.Clear();
            foreach (FileResult result in this.controller.Results)
            {
                var listItem =
                    new ListViewItem(
                        new[]
                            {
                                result.InputFile.Name, result.Show == null ? string.Empty : result.Show.Name, 
                                result.Episode == null
                                    ? string.Empty
                                    : result.Episode.SeasonNumber.ToString(CultureInfo.InvariantCulture), 
                                result.Episode == null
                                    ? string.Empty
                                    : result.Episode.EpisodeNumber.ToString(CultureInfo.InvariantCulture), 
                                result.Episode == null ? string.Empty : result.Episode.Name, result.OutputPath
                            });
                if (result.Incomplete)
                {
                    listItem.BackColor = Color.Red;
                }
                else
                {
                    listItem.BackColor = Color.White;
                }

                this.resultsList.Items.Add(listItem);
            }
        }

        /// <summary>
        /// Resets the data for the specified items.
        /// </summary>
        /// <param name="checkedResults">
        /// The collection of indices of items to update. 
        /// </param>
        private void ResetListItems(IEnumerable<int> checkedResults)
        {
            foreach (int result in checkedResults)
            {
                ListViewItem item = this.resultsList.Items[result];
                item.SubItems[0].Text = this.controller.Results[result].InputFile.FullName;
                item.SubItems[1].Text = this.controller.Results[result].Show.Name;
                item.SubItems[2].Text = this.controller.Results[result].Episode != null
                                            ? this.controller.Results[result].Episode.EpisodeNumber.ToString(
                                                CultureInfo.InvariantCulture)
                                            : string.Empty;
                item.SubItems[3].Text = this.controller.Results[result].Episode != null
                                            ? this.controller.Results[result].Episode.SeasonNumber.ToString(
                                                CultureInfo.InvariantCulture)
                                            : string.Empty;
                item.SubItems[4].Text = this.controller.Results[result].Episode != null
                                            ? this.controller.Results[result].Episode.Name
                                            : string.Empty;
                item.SubItems[5].Text = this.controller.Results[result].OutputPath;
                if (this.controller.Results[result].Show == null || this.controller.Results[result].Episode == null
                    || string.IsNullOrEmpty(this.controller.Results[result].Episode.Name))
                {
                    item.BackColor = Color.Red;
                }
                else
                {
                    item.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Handles the Scan button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ScanButtonClick(object sender, EventArgs e)
        {
            this.controller.ScanEpisodes((string)this.subDirectoryFilter.SelectedItem);
        }

        /// <summary>
        /// Handles the Select All button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SelectAllButtonClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.resultsList.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        /// Handles the Set Episode button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SetEpisodeButtonClick(object sender, EventArgs e)
        {
            var dialog = new NumberInputDialog { Text = "Set episode" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                List<int> checkedResults = this.resultsList.CheckedIndices.Cast<int>().ToList();
                this.controller.SetEpisodeNum(checkedResults, dialog.Number);
                this.ResetListItems(checkedResults);
            }
        }

        /// <summary>
        /// Handles the Set Season button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SetSeasonButtonClick(object sender, EventArgs e)
        {
            var dialog = new NumberInputDialog { Text = "Set season" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                List<int> checkedResults = this.resultsList.CheckedIndices.Cast<int>().ToList();
                this.controller.SetSeasonNum(checkedResults, dialog.Number);
                this.ResetListItems(checkedResults);
            }
        }

        /// <summary>
        /// Handles the Set show button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SetShowButtonClick(object sender, EventArgs e)
        {
            var dialog = new ShowSelectDialog(this.controller.Shows);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                List<int> checkedResults = this.resultsList.CheckedIndices.Cast<int>().ToList();
                this.controller.SetShow(checkedResults, dialog.SelectedShow);

                // Update the listitems with the altered results
                this.ResetListItems(checkedResults);
            }
        }

        /// <summary>
        /// Handles the tab loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SortEpisodesLoad(object sender, EventArgs e)
        {
            this.controller.Initialise(this);
        }

        #endregion
    }
}