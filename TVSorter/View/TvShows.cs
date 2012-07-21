// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShows.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The TV Shows tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using TVSorter.Controller;
    using TVSorter.Model;
    using TVSorter.Properties;

    #endregion

    /// <summary>
    /// The TV Shows tab.
    /// </summary>
    public partial class TvShows : UserControl, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private TvShowsController controller;

        /// <summary>
        ///   A value indicating whether the select show is locked.
        /// </summary>
        private bool selectedShowLocked;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TvShows" /> class.
        /// </summary>
        public TvShows()
        {
            this.InitializeComponent();
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
        /// Handles the Add show button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void AddShowButtonClick(object sender, EventArgs e)
        {
            var dialog = new ShowSearchDialog(new AddShowController());
            dialog.ShowDialog(this);
        }

        /// <summary>
        /// Handles the Alternatee names button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void AlternateNamesButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(this.controller.SelectedShow.AlternateNames, "Alternate Names");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.controller.SelectedShow.AlternateNames = listController.List.ToList();
            }
        }

        /// <summary>
        /// Handles the Create NFO files button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void CreateNfoFilesButtonClick(object sender, EventArgs e)
        {
            this.controller.CreateNfoFiles();
        }

        /// <summary>
        /// Handles the Format builder button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void FormatBuilderClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles a property in the controller changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.PropertyChanged(sender, e)));
            }
            else
            {
                switch (e.PropertyName)
                {
                    case "Shows":
                        this.tvShowsList.DisplayMember = "Name";
                        this.tvShowsList.DataSource = this.controller.Shows;
                        break;
                    case "SelectedShow":
                        if (this.controller.SelectedShow != null)
                        {
                            this.selectedShowName.Text = this.controller.SelectedShow.Name;
                            string imagePath = "Images" + Path.DirectorySeparatorChar
                                               + this.controller.SelectedShow.TvdbId + ".jpg";
                            try
                            {
                                this.selectedShowBanner.Image = File.Exists(imagePath)
                                                                    ? Image.FromFile(imagePath)
                                                                    : null;
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    File.Delete(imagePath);
                                }
                                catch
                                {
                                    // Unable to delete
                                }
                            }

                            this.selectedShowTvdb.Text = "TVDB ID: " + this.controller.SelectedShow.TvdbId;
                            this.selectedShowLastUpdated.Text = "Last Updated: "
                                                                + this.controller.SelectedShow.LastUpdated;
                            this.selectedShowUseCustomFormat.Checked = this.controller.SelectedShow.UseCustomFormat;
                            this.selectedShowCustomFormatText.Text = this.controller.SelectedShow.CustomFormat;
                            this.selectedShowFolderNameText.Text = this.controller.SelectedShow.FolderName;
                            this.alternateNamesButton.Enabled = true;
                            this.selectedShowUseDvdOrder.Checked = this.controller.SelectedShow.UseDvdOrder;
                            this.selectedShowLocked = this.controller.SelectedShow.Locked;
                            this.selectedShowLockButton.Text = this.selectedShowLocked ? "Unlock Show" : "Lock Show";
                            this.selectedShowLockButton.BackColor = this.selectedShowLocked ? Color.Red : Color.Green;
                            this.selectedShowLockButton.Enabled = true;
                        }
                        else
                        {
                            this.selectedShowName.Text = string.Empty;
                            this.selectedShowBanner.Image = null;
                            this.selectedShowTvdb.Text = "TVDB ID: ";
                            this.selectedShowLastUpdated.Text = "Last Updated: ";
                            this.selectedShowUseCustomFormat.Checked = false;
                            this.selectedShowCustomFormatText.Text = string.Empty;
                            this.selectedShowFolderNameText.Text = string.Empty;
                            this.alternateNamesButton.Enabled = false;
                            this.selectedShowUseDvdOrder.Checked = false;
                            this.selectedShowLocked = false;
                            this.selectedShowLockButton.Text = "Unlock Show";
                            this.selectedShowLockButton.BackColor = Color.Red;
                            this.selectedShowLockButton.Enabled = false;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Remove button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RemoveShowButtonClick(object sender, EventArgs e)
        {
            this.controller.RemoveSelectedShow();
        }

        /// <summary>
        /// Handles the reset last updated button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ResetLastUpdatedButtonClick(object sender, EventArgs e)
        {
            this.controller.ResetLastUpdated();
        }

        /// <summary>
        /// Handles the Revert button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RevertButtonClick(object sender, EventArgs e)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs("SelectedShow"));
        }

        /// <summary>
        /// Handles the Save button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (this.controller.SelectedShow == null)
            {
                return;
            }

            this.controller.SelectedShow.CustomFormat = this.selectedShowCustomFormatText.Text;
            this.controller.SelectedShow.FolderName = this.selectedShowFolderNameText.Text;
            this.controller.SelectedShow.UseCustomFormat = this.selectedShowUseCustomFormat.Checked;
            this.controller.SelectedShow.UseDvdOrder = this.selectedShowUseDvdOrder.Checked;
            this.controller.SelectedShow.Locked = this.selectedShowLocked;
            this.controller.SaveSelectedShow();

            this.tvShowsList.Refresh();
        }

        /// <summary>
        /// Handles the Search shows button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SearchShowsButtonClick(object sender, EventArgs e)
        {
            this.controller.SearchShows();
        }

        /// <summary>
        /// Handles the search for shows completing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SearchShowsComplete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.SearchShowsComplete(sender, e)));
            }
            else
            {
                foreach (var result in this.controller.SearchResults)
                {
                    var searchResultsController = new SearchResultsController();
                    var dialog = new ShowSearchDialog(searchResultsController);
                    searchResultsController.SetResults(result.Key, result.Value);
                    dialog.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Handles the show lock button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SelectedShowLockButtonClick(object sender, EventArgs e)
        {
            this.selectedShowLocked = !this.selectedShowLocked;
            this.selectedShowLockButton.Text = this.selectedShowLocked ? "Unlock Show" : "Lock Show";
            this.selectedShowLockButton.BackColor = this.selectedShowLocked ? Color.Red : Color.Green;
        }

        /// <summary>
        /// Handles the checked state of the use custom format checkbox changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SelectedUseCustomFormatCheckedChanged(object sender, EventArgs e)
        {
            this.selectedShowCustomFormatText.Enabled = this.selectedShowUseCustomFormat.Checked;
            this.formatBuilder.Enabled = this.selectedShowUseCustomFormat.Checked;
        }

        /// <summary>
        /// Handles drawing an item in the list.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void TvShowsListDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= this.controller.Shows.Count)
            {
                return;
            }

            TvShow show = this.controller.Shows[e.Index];
            if (show == null)
            {
                return;
            }

            // Draw the background color depending on 
            // if the item is selected or not.
            Color textColour;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // The item is selected.
                // We want a highlight color background. This should match the user's theme
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Highlight)), e.Bounds);
                textColour = Color.FromKnownColor(KnownColor.HighlightText);
            }
            else
            {
                // The item is NOT selected.
                // We want a white background color.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Window)), e.Bounds);
                textColour = Color.FromKnownColor(KnownColor.WindowText);
            }

            if (show.Locked)
            {
                e.Graphics.DrawImage(Resources._lock, new PointF(5, (2 + e.Bounds.Y) + ((e.Bounds.Height - 20) / 2.0f)));
            }

            SizeF stringSize = e.Graphics.MeasureString(show.Name, e.Font);

            // Draw the text, X is 25 if locked to allow for padlock else 5
            e.Graphics.DrawString(
                show.Name, 
                e.Font, 
                new SolidBrush(textColour), 
                new PointF(show.Locked ? 25 : 5, e.Bounds.Y + ((e.Bounds.Height - stringSize.Height) / 2.0f)));
        }

        /// <summary>
        /// Handles the TV shows list selected item changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void TvShowsListSelectedIndexChanged(object sender, EventArgs e)
        {
            this.controller.TvShowSelected(this.tvShowsList.SelectedIndex);
        }

        /// <summary>
        /// Handles the TV Shows tab loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void TvShowsLoad(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.controller = new TvShowsController();
                this.controller.PropertyChanged += this.PropertyChanged;
                this.controller.SearchShowsComplete += this.SearchShowsComplete;
                this.controller.Initialise(this);
            }
        }

        /// <summary>
        /// Handles the Update all button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void UpdateAllButtonClick(object sender, EventArgs e)
        {
            this.controller.UpdateAllShows();
        }

        /// <summary>
        /// Handles the Update Show button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void UpdateShowButtonClick(object sender, EventArgs e)
        {
            this.controller.UpdateSelectedShow();
        }

        #endregion
    }
}