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
    using System.ComponentModel;
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
        private SortEpisodesController controller;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortEpisodes" /> class.
        /// </summary>
        public SortEpisodes()
        {
            this.InitializeComponent();
            this.setEpisodeButton.Enabled = false;
            this.setShowButton.Enabled = false;
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
            this.resultsList.Items.AddRange(this.controller.Results.Select(x => x.GetListViewItem()).ToArray());
        }

        /// <summary>
        /// Resets the data for the specified items.
        /// </summary>
        private void ResetListItems()
        {
            for (int i = 0; i < this.controller.Results.Count; i++)
            {
                this.resultsList.Items[i] = this.controller.Results[i].GetListViewItem();
            }
        }

        /// <summary>
        /// Handles an item being checked or unchecked in the results list.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void ResultsListItemChecked(object sender, ItemCheckedEventArgs e)
        {
            this.controller.Results[e.Item.Index].Checked = e.Item.Checked;

            if (this.controller.Results.Count(p => p.Checked) > 0)
                setShowButton.Enabled = true;
            if (this.controller.Results.Count(p => p.Checked) == 1)
                setEpisodeButton.Enabled = true;
            if (this.controller.Results.Count(p => p.Checked) > 1)
                setEpisodeButton.Enabled = false;
            else if (this.controller.Results.Count(p => p.Checked) == 0)
            {
                setEpisodeButton.Enabled = false;
                setShowButton.Enabled = false;
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
            var dialog = new NumberInputDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.controller.SetEpisode(dialog.SeasonNumber, dialog.EpisodeNumber);
                this.ResetListItems();
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
                this.controller.SetShow(dialog.SelectedShow);

                // Update the listitems with the altered results
                this.ResetListItems();
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
            if (!this.DesignMode)
            {
                this.controller = new SortEpisodesController();
                this.controller.PropertyChanged += this.ControllerOnPropertyChanged;
                this.controller.Initialise(this);
            }
        }

        #endregion
    }
}