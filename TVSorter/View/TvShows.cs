// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShows.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The TV Shows tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TVSorter.Controller;
using TVSorter.Files;
using TVSorter.Properties;
using TVSorter.Repostitory;

namespace TVSorter.View
{
    /// <summary>
    ///     The TV Shows tab.
    /// </summary>
    public partial class TvShows : UserControl, IView
    {
        private readonly IFileResultManager fileResultManager;
        private readonly ITvShowRepository tvShowRepository;

        /// <summary>
        ///     The list of alternate names.
        /// </summary>
        private List<string> alternateNames;

        /// <summary>
        ///     The controller.
        /// </summary>
        private TvShowsController controller;

        /// <summary>
        ///     A value indicating whether the select show is locked.
        /// </summary>
        private bool selectedShowLocked;

        /// <summary>
        ///     Initialises a new instance of the <see cref="TvShows" /> class.
        /// </summary>
        public TvShows()
        {
            InitializeComponent();

            tvShowRepository = CompositionRoot.Get<ITvShowRepository>();
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
        ///     Handles the Add show button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void AddShowButtonClick(object sender, EventArgs e)
        {
            var dialog = new ShowSearchDialog(new AddShowController(tvShowRepository));
            dialog.ShowDialog(this);
        }

        /// <summary>
        ///     Handles the Alternate names button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void AlternateNamesButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(alternateNames, "Alternate Names");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                alternateNames = listController.List.ToList();
            }
        }

        /// <summary>
        ///     Handles the Format builder button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void FormatBuilderClick(object sender, EventArgs e)
        {
            var formatBuilderDialog = new FormatBuilder(selectedShowCustomFormatText.Text, fileResultManager);
            if (formatBuilderDialog.ShowDialog(this) == DialogResult.OK)
            {
                selectedShowCustomFormatText.Text = formatBuilderDialog.FormatString;
            }
        }

        /// <summary>
        ///     Handles a property in the controller changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => PropertyChanged(sender, e)));
            }
            else
            {
                switch (e.PropertyName)
                {
                    case "Shows":
                        tvShowsList.DisplayMember = "Name";
                        tvShowsList.DataSource = controller.Shows;
                        break;
                    case "SelectedShow":
                        if (controller.SelectedShow != null)
                        {
                            selectedShowName.Text = controller.SelectedShow.Name;
                            var imagePath = "Images" +
                                            Path.DirectorySeparatorChar +
                                            controller.SelectedShow.TvdbId +
                                            ".jpg";
                            try
                            {
                                selectedShowBanner.Image = File.Exists(imagePath) ? Image.FromFile(imagePath) : null;
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

                            selectedShowTvdb.Text = "TVDB ID: " + controller.SelectedShow.TvdbId;
                            selectedShowLastUpdated.Text = "Last Updated: " + controller.SelectedShow.LastUpdated;
                            selectedShowUseCustomFormat.Checked = controller.SelectedShow.UseCustomFormat;
                            selectedShowCustomFormatText.Text = controller.SelectedShow.CustomFormat;
                            selectedShowFolderNameText.Text = controller.SelectedShow.FolderName;
                            alternateNamesButton.Enabled = true;
                            selectedShowUseDvdOrder.Checked = controller.SelectedShow.UseDvdOrder;
                            selectedShowLocked = controller.SelectedShow.Locked;
                            selectedShowLockButton.Text = selectedShowLocked ? "Unlock Show" : "Lock Show";
                            selectedShowLockButton.BackColor = selectedShowLocked ? Color.Red : Color.Green;
                            selectedShowLockButton.Enabled = true;
                            alternateNames = new List<string>(controller.SelectedShow.AlternateNames);
                            useCustomDestinationDirectory.Checked = controller.SelectedShow.UseCustomDestination;
                            customDestination.SelectedItem = controller.SelectedShow.CustomDestinationDir;
                        }
                        else
                        {
                            selectedShowName.Text = string.Empty;
                            selectedShowBanner.Image = null;
                            selectedShowTvdb.Text = "TVDB ID: ";
                            selectedShowLastUpdated.Text = "Last Updated: ";
                            selectedShowUseCustomFormat.Checked = false;
                            selectedShowCustomFormatText.Text = string.Empty;
                            selectedShowFolderNameText.Text = string.Empty;
                            alternateNamesButton.Enabled = false;
                            selectedShowUseDvdOrder.Checked = false;
                            selectedShowLocked = false;
                            selectedShowLockButton.Text = "Unlock Show";
                            selectedShowLockButton.BackColor = Color.Red;
                            selectedShowLockButton.Enabled = false;
                            useCustomDestinationDirectory.Checked = false;
                            customDestination.SelectedItem = null;
                        }

                        break;
                    case "DestinationDirectories":
                        customDestination.DataSource = controller.DestinationDirectories;
                        break;
                }
            }
        }

        /// <summary>
        ///     Handles the Remove button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RemoveShowButtonClick(object sender, EventArgs e)
        {
            controller.RemoveSelectedShow();
        }

        /// <summary>
        ///     Handles the reset last updated button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ResetLastUpdatedButtonClick(object sender, EventArgs e)
        {
            controller.ResetLastUpdated();
        }

        /// <summary>
        ///     Handles the Revert button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RevertButtonClick(object sender, EventArgs e)
        {
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedShow"));
        }

        /// <summary>
        ///     Handles the Save button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SaveButtonClick(object sender, EventArgs e)
        {
            if (controller.SelectedShow == null)
            {
                return;
            }

            controller.SelectedShow.CustomFormat = selectedShowCustomFormatText.Text;
            controller.SelectedShow.FolderName = selectedShowFolderNameText.Text;
            controller.SelectedShow.UseCustomFormat = selectedShowUseCustomFormat.Checked;
            controller.SelectedShow.UseDvdOrder = selectedShowUseDvdOrder.Checked;
            controller.SelectedShow.Locked = selectedShowLocked;
            controller.SelectedShow.AlternateNames = alternateNames;
            controller.SelectedShow.UseCustomDestination = useCustomDestinationDirectory.Checked;
            controller.SelectedShow.CustomDestinationDir = (string)customDestination.SelectedItem ?? string.Empty;
            controller.SaveSelectedShow();

            tvShowsList.Refresh();
        }

        /// <summary>
        ///     Handles the Search shows button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SearchShowsButtonClick(object sender, EventArgs e)
        {
            controller.SearchShows();
        }

        /// <summary>
        ///     Handles the search for shows completing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SearchShowsComplete(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SearchShowsComplete(sender, e)));
            }
            else
            {
                foreach (var result in controller.SearchResults)
                {
                    var searchResultsController = new SearchResultsController(tvShowRepository);
                    var dialog = new ShowSearchDialog(searchResultsController);
                    searchResultsController.SetResults(result.Key, result.Value);
                    dialog.ShowDialog(this);
                }
            }
        }

        /// <summary>
        ///     Handles the show lock button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SelectedShowLockButtonClick(object sender, EventArgs e)
        {
            selectedShowLocked = !selectedShowLocked;
            selectedShowLockButton.Text = selectedShowLocked ? "Unlock Show" : "Lock Show";
            selectedShowLockButton.BackColor = selectedShowLocked ? Color.Red : Color.Green;
        }

        /// <summary>
        ///     Handles the checked state of the use custom format checkbox changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SelectedUseCustomFormatCheckedChanged(object sender, EventArgs e)
        {
            selectedShowCustomFormatText.Enabled = selectedShowUseCustomFormat.Checked;
            formatBuilder.Enabled = selectedShowUseCustomFormat.Checked;
        }

        /// <summary>
        ///     Handles the controller's ShowChanged event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ShowChanged(object sender, TvShowEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowChanged(sender, e)));
            }
            else
            {
                tvShowsList.Refresh();
            }
        }

        /// <summary>
        ///     Handles drawing an item in the list.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void TvShowsListDrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.Index < 0) || (e.Index >= controller.Shows.Count))
            {
                return;
            }

            var show = controller.Shows[e.Index];
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
                e.Graphics.DrawImage(Resources._lock, new PointF(5, 2 + e.Bounds.Y + ((e.Bounds.Height - 20) / 2.0f)));
            }

            var stringSize = e.Graphics.MeasureString(show.Name, e.Font);

            // Draw the text, X is 25 if locked to allow for padlock else 5
            e.Graphics.DrawString(
                show.Name,
                e.Font,
                new SolidBrush(textColour),
                new PointF(show.Locked ? 25 : 5, e.Bounds.Y + ((e.Bounds.Height - stringSize.Height) / 2.0f)));
        }

        /// <summary>
        ///     Handles the TV shows list selected item changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void TvShowsListSelectedIndexChanged(object sender, EventArgs e)
        {
            controller.TvShowSelected(tvShowsList.SelectedIndex);
        }

        /// <summary>
        ///     Handles the TV Shows tab loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void TvShowsLoad(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                controller = CompositionRoot.Get<TvShowsController>();
                controller.PropertyChanged += PropertyChanged;
                controller.ShowChanged += ShowChanged;
                controller.SearchShowsComplete += SearchShowsComplete;
                controller.Initialise(this);
            }
        }

        /// <summary>
        ///     Handles the Update all button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void UpdateAllButtonClick(object sender, EventArgs e)
        {
            controller.UpdateAllShows();
        }

        /// <summary>
        ///     Handles the Update Show button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void UpdateShowButtonClick(object sender, EventArgs e)
        {
            controller.UpdateSelectedShow();
        }

        /// <summary>
        ///     The the check changed event of the Use Custom Destination Directory check box.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void UseCustomDestinationDirectoryCheckedChanged(object sender, EventArgs e)
        {
            customDestination.Enabled = useCustomDestinationDirectory.Checked;
        }
    }
}
