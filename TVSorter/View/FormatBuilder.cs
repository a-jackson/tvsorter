// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatBuilder.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The class for the FormatBuilder form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using Files;
    using Model;

    /// <summary>
    /// The class for the FormatBuilder form.
    /// </summary>
    public partial class FormatBuilder : Form
    {
        #region Fields

        /// <summary>
        /// The result used for the example format.
        /// </summary>
        private readonly FileResult exampleResult;

        /// <summary>
        /// The file result manager.
        /// </summary>
        private readonly IFileResultManager fileResultManager;
        
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FormatBuilder"/> class.
        /// </summary>
        /// <param name="currentFormat">
        /// The current format.
        /// </param>
        /// <param name="fileResultManager">
        /// The file result manager.
        /// </param>
        public FormatBuilder(string currentFormat, IFileResultManager fileResultManager)
        {
            this.InitializeComponent();
            this.fileResultManager = fileResultManager;
            this.exampleResult = FileResult.Example;
            this.textFormat.Text = currentFormat;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the format string.
        /// </summary>
        public string FormatString { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the click event for the Cancel button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handles the click event for the Date button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void DateButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{Date(" + this.dateFormat.Text + ")}";
        }

        /// <summary>
        /// Handles the text of the date format changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void DateFormatTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.dateExample.Text = DateTime.Today.ToString(this.dateFormat.Text);
            }
            catch
            {
                this.dateExample.Text = "Error";
            }
        }

        /// <summary>
        /// Handles the click event for the Directory button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void DirectoryButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Handles the click event for the Episode Name button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void EpisodeNameButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{EName(" + this.GetSeparator() + ")}";
        }

        /// <summary>
        /// Handles the click event Episode Number for the  button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void EpisodeNumberButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{ENum(" + this.digitalSelector.Value + ")}";
        }

        /// <summary>
        /// Handles the click event for the File Extension button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void FileExtensionButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{Ext}";
        }

        /// <summary>
        /// Handles the click event for the Folder Name button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void FolderNameButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{FName}";
        }

        /// <summary>
        /// Handles the load event for the form.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void FormatBuilderLoad(object sender, EventArgs e)
        {
            this.wordSeparator.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets the word separator character.
        /// </summary>
        /// <returns>The word separator character.</returns>
        private string GetSeparator()
        {
            switch (this.wordSeparator.SelectedItem.ToString())
            {
                case "[space]":
                    return " ";
                default:
                    return this.wordSeparator.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Handles the click event for the OK button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OkButtonClick(object sender, EventArgs e)
        {
            this.FormatString = this.textFormat.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the click event for the Season Number button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void SeasonNumberButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{SNum(" + this.digitalSelector.Value + ")}";
        }

        /// <summary>
        /// Handles the click event for the Show Name button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void ShowNameButtonClick(object sender, EventArgs e)
        {
            this.textFormat.Text += "{SName(" + this.GetSeparator() + ")}";
        }

        /// <summary>
        /// Handles the text for changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void TextFormatTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.textExample.Text = fileResultManager.FormatOutputPath(this.exampleResult, this.textFormat.Text);
            }
            catch
            {
                this.textExample.Text = "Error";
            }
        }

        #endregion
    }
}