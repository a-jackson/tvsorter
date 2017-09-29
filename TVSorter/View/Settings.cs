// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TVSorter.Controller;
using TVSorter.Files;

namespace TVSorter.View
{
    /// <summary>
    ///     The settings tab.
    /// </summary>
    public partial class Settings : UserControl, IView
    {
        private readonly IFileResultManager fileResultManager;

        /// <summary>
        ///     The controller.
        /// </summary>
        private SettingsController controller;

        /// <summary>
        ///     The list of destination directories.
        /// </summary>
        private BindingList<string> destinationDirectories;

        /// <summary>
        ///     The list of file extensions.
        /// </summary>
        private List<string> fileExtensions;

        /// <summary>
        ///     The list of Ignored directories.
        /// </summary>
        private BindingList<string> ignoredDirectories;

        /// <summary>
        ///     The list of overwrite keywords.
        /// </summary>
        private List<string> overwriteKeywords;

        /// <summary>
        ///     The list of regular expressions.
        /// </summary>
        private List<string> regularExpressions;

        /// <summary>
        ///     Initialises a new instance of the <see cref="Settings" /> class.
        /// </summary>
        public Settings()
        {
            InitializeComponent();

            fileResultManager = CompositionRoot.Get<FileResultManager>();
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
            // Not needed.
        }

        /// <summary>
        ///     Handles the Add destination button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void AddDestinationButtonClick(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                destinationDirectories.Add(folderDialog.SelectedPath);
            }
        }

        /// <summary>
        ///     Handles the Add Ignore button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void AddIgnoreButtonClick(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                ignoredDirectories.Add(folderDialog.SelectedPath);
            }
        }

        /// <summary>
        ///     Handles the click event of the Edit Overwrite Keywords button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void EditOverwriteKeywordsButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(overwriteKeywords, "Overwrite keywords");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                overwriteKeywords = listController.List.ToList();
            }
        }

        /// <summary>
        ///     Handles the File Extensions button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void FileExtensionsButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(fileExtensions, "File Extensions");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                fileExtensions = listController.List.ToList();
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
        private void FormatBuilderButtonClick(object sender, EventArgs e)
        {
            var formatBuilderDialog = new FormatBuilder(formatText.Text, fileResultManager);
            if (formatBuilderDialog.ShowDialog(this) == DialogResult.OK)
            {
                formatText.Text = formatBuilderDialog.FormatString;
            }
        }

        /// <summary>
        ///     Handles a property changing.
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
                case "Settings":

                    // Create lists
                    destinationDirectories = new BindingList<string>(controller.Settings.DestinationDirectories);
                    ignoredDirectories = new BindingList<string>(controller.Settings.IgnoredDirectories);
                    regularExpressions = new List<string>(controller.Settings.RegularExpressions);
                    fileExtensions = new List<string>(controller.Settings.FileExtensions);
                    overwriteKeywords = new List<string>(controller.Settings.OverwriteKeywords);

                    // Update directories
                    sourceText.Text = controller.Settings.SourceDirectory;
                    destinationList.DataSource = destinationDirectories;
                    ignoreList.DataSource = ignoredDirectories;
                    defaultDestinationDirectory.DataSource = destinationDirectories;
                    defaultDestinationDirectory.SelectedItem = controller.Settings.DefaultDestinationDirectory;

                    // Checkboxes
                    recurseSubdirectoriesCheck.Checked = controller.Settings.RecurseSubdirectories;
                    deleteEmptyCheck.Checked = controller.Settings.DeleteEmptySubdirectories;
                    renameIfExistsCheck.Checked = controller.Settings.RenameIfExists;
                    unlockAndUpdateCheck.Checked = controller.Settings.UnlockMatchedShows;
                    addUnmatchedShowsCheck.Checked = controller.Settings.AddUnmatchedShows;
                    lockShowWithNoNewEpisodesCheck.Checked = controller.Settings.LockShowsWithNoEpisodes;

                    // Format text
                    formatText.Text = controller.Settings.DefaultOutputFormat;
                    break;
            }
        }

        /// <summary>
        ///     Handles the Regular Expressions button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RegExButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(regularExpressions, "Regular Expressions");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                regularExpressions = listController.List.ToList();
            }
        }

        /// <summary>
        ///     Handles the Remove destination button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RemoveDestinationButtonClick(object sender, EventArgs e)
        {
            if (destinationList.SelectedIndex != -1)
            {
                destinationDirectories.RemoveAt(destinationList.SelectedIndex);
            }
        }

        /// <summary>
        ///     Handles the Remove ignore button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RemoveIgnoreButtonClick(object sender, EventArgs e)
        {
            if (destinationList.SelectedIndex != -1)
            {
                ignoredDirectories.RemoveAt(ignoreList.SelectedIndex);
            }
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
            controller.Revert();
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
            controller.Settings.RegularExpressions = regularExpressions.ToList();
            controller.Settings.FileExtensions = fileExtensions.ToList();
            controller.Settings.DestinationDirectories = destinationDirectories.ToList();
            controller.Settings.IgnoredDirectories = ignoredDirectories.ToList();
            controller.Settings.OverwriteKeywords = overwriteKeywords.ToList();

            controller.Settings.SourceDirectory = sourceText.Text;
            controller.Settings.DefaultDestinationDirectory =
                (string)defaultDestinationDirectory.SelectedItem ?? string.Empty;

            controller.Settings.RecurseSubdirectories = recurseSubdirectoriesCheck.Checked;
            controller.Settings.DeleteEmptySubdirectories = deleteEmptyCheck.Checked;
            controller.Settings.RenameIfExists = renameIfExistsCheck.Checked;
            controller.Settings.AddUnmatchedShows = addUnmatchedShowsCheck.Checked;
            controller.Settings.UnlockMatchedShows = unlockAndUpdateCheck.Checked;
            controller.Settings.LockShowsWithNoEpisodes = lockShowWithNoNewEpisodesCheck.Checked;

            controller.Settings.DefaultOutputFormat = formatText.Text;

            controller.Save();
        }

        /// <summary>
        ///     Handles the Settings tab loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SettingsLoad(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                controller = CompositionRoot.Get<SettingsController>();
                controller.PropertyChanged += PropertyChanged;
                controller.Initialise(this);
            }
        }

        /// <summary>
        ///     Handles the Source Browse button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SourceBrowseClick(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                sourceText.Text = folderDialog.SelectedPath;
            }
        }
    }
}
