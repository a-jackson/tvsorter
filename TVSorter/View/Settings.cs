// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;

    using Controller;
    using Files;
    using Storage;

    #endregion

    /// <summary>
    /// The settings tab.
    /// </summary>
    public partial class Settings : UserControl, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private SettingsController controller;

        /// <summary>
        ///   The list of destination directories.
        /// </summary>
        private BindingList<string> destinationDirectories;

        /// <summary>
        ///   The list of Ignored directories.
        /// </summary>
        private BindingList<string> ignoredDirectories;

        /// <summary>
        ///   The list of file extensions.
        /// </summary>
        private List<string> fileExtensions;

        /// <summary>
        /// The list of overwrite keywords.
        /// </summary>
        private List<string> overwriteKeywords;

        /// <summary>
        ///   The list of regular expressions.
        /// </summary>
        private List<string> regularExpressions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initialises a new instance of the <see cref="Settings" /> class.
        /// </summary>
        public Settings()
        {
            this.InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Gets or sets the storage provider.
        /// </summary>
        public IStorageProvider StorageProvider { get; set; }

        /// <summary>
        /// Gets or sets the file result manager.
        /// </summary>
        public IFileResultManager FileResultManager { get; set; }

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
            // Not needed.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the Add destination button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void AddDestinationButtonClick(object sender, EventArgs e)
        {
            if (this.folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.destinationDirectories.Add(this.folderDialog.SelectedPath);
            }
        }

        /// <summary>
        /// Handles the Add Ignore button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void AddIgnoreButtonClick(object sender, EventArgs e)
        {
            if (this.folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.ignoredDirectories.Add(this.folderDialog.SelectedPath);
            }
        }

        /// <summary>
        /// Handles the click event of the Edit Overwrite Keywords button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void EditOverwriteKeywordsButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(this.overwriteKeywords, "Overwrite keywords");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.overwriteKeywords = listController.List.ToList();
            }
        }

        /// <summary>
        /// Handles the File Extensions button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void FileExtensionsButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(this.fileExtensions, "File Extensions");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.fileExtensions = listController.List.ToList();
            }
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
        private void FormatBuilderButtonClick(object sender, EventArgs e)
        {
            var formatBuilderDialog = new FormatBuilder(this.formatText.Text, FileResultManager);
            if (formatBuilderDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.formatText.Text = formatBuilderDialog.FormatString;
            }
        }

        /// <summary>
        /// Handles a property changing.
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
                case "Settings":

                    // Create lists
                    this.destinationDirectories =
                        new BindingList<string>(this.controller.Settings.DestinationDirectories);
                    this.ignoredDirectories =
                        new BindingList<string>(this.controller.Settings.IgnoredDirectories);
                    this.regularExpressions = new List<string>(this.controller.Settings.RegularExpressions);
                    this.fileExtensions = new List<string>(this.controller.Settings.FileExtensions);
                    this.overwriteKeywords = new List<string>(this.controller.Settings.OverwriteKeywords);

                    // Update directories
                    this.sourceText.Text = this.controller.Settings.SourceDirectory;
                    this.destinationList.DataSource = this.destinationDirectories;
                    this.ignoreList.DataSource = this.ignoredDirectories;
                    this.defaultDestinationDirectory.DataSource = this.destinationDirectories;
                    this.defaultDestinationDirectory.SelectedItem = this.controller.Settings.DefaultDestinationDirectory;

                    // Checkboxes
                    this.recurseSubdirectoriesCheck.Checked = this.controller.Settings.RecurseSubdirectories;
                    this.deleteEmptyCheck.Checked = this.controller.Settings.DeleteEmptySubdirectories;
                    this.renameIfExistsCheck.Checked = this.controller.Settings.RenameIfExists;
                    this.unlockAndUpdateCheck.Checked = this.controller.Settings.UnlockMatchedShows;
                    this.addUnmatchedShowsCheck.Checked = this.controller.Settings.AddUnmatchedShows;
                    this.lockShowWithNoNewEpisodesCheck.Checked = this.controller.Settings.LockShowsWithNoEpisodes;

                    // Format text
                    this.formatText.Text = this.controller.Settings.DefaultOutputFormat;
                    break;
            }
        }

        /// <summary>
        /// Handles the Regular Expressions button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RegExButtonClick(object sender, EventArgs e)
        {
            var listController = new ListController(this.regularExpressions, "Regular Expressions");
            var dialog = new ListDialog(listController);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.regularExpressions = listController.List.ToList();
            }
        }

        /// <summary>
        /// Handles the Remove destination button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RemoveDestinationButtonClick(object sender, EventArgs e)
        {
            if (this.destinationList.SelectedIndex != -1)
            {
                this.destinationDirectories.RemoveAt(this.destinationList.SelectedIndex);
            }
        }

        /// <summary>
        /// Handles the Remove ignore button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RemoveIgnoreButtonClick(object sender, EventArgs e)
        {
            if (this.destinationList.SelectedIndex != -1)
            {
                this.ignoredDirectories.RemoveAt(this.ignoreList.SelectedIndex);
            }
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
            this.controller.Revert();
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
            this.controller.Settings.RegularExpressions = this.regularExpressions.ToList();
            this.controller.Settings.FileExtensions = this.fileExtensions.ToList();
            this.controller.Settings.DestinationDirectories = this.destinationDirectories.ToList();
            this.controller.Settings.IgnoredDirectories = this.ignoredDirectories.ToList();
            this.controller.Settings.OverwriteKeywords = this.overwriteKeywords.ToList();

            this.controller.Settings.SourceDirectory = this.sourceText.Text;
            this.controller.Settings.DefaultDestinationDirectory = (string)this.defaultDestinationDirectory.SelectedItem ?? string.Empty;

            this.controller.Settings.RecurseSubdirectories = this.recurseSubdirectoriesCheck.Checked;
            this.controller.Settings.DeleteEmptySubdirectories = this.deleteEmptyCheck.Checked;
            this.controller.Settings.RenameIfExists = this.renameIfExistsCheck.Checked;
            this.controller.Settings.AddUnmatchedShows = this.addUnmatchedShowsCheck.Checked;
            this.controller.Settings.UnlockMatchedShows = this.unlockAndUpdateCheck.Checked;
            this.controller.Settings.LockShowsWithNoEpisodes = this.lockShowWithNoNewEpisodesCheck.Checked;

            this.controller.Settings.DefaultOutputFormat = this.formatText.Text;

            this.controller.Save();
        }

        /// <summary>
        /// Handles the Settings tab loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SettingsLoad(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.controller = new SettingsController(StorageProvider);
                this.controller.PropertyChanged += this.PropertyChanged;
                this.controller.Initialise(this);
            }
        }

        /// <summary>
        /// Handles the Source Browse button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void SourceBrowseClick(object sender, EventArgs e)
        {
            if (this.folderDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.sourceText.Text = this.folderDialog.SelectedPath;
            }
        }

        #endregion
    }
}