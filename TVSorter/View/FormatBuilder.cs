// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatBuilder.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The class for the FormatBuilder form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using TVSorter.Files;
using TVSorter.Model;

namespace TVSorter.View
{
    /// <summary>
    ///     The class for the FormatBuilder form.
    /// </summary>
    public partial class FormatBuilder : Form
    {
        /// <summary>
        ///     The result used for the example format.
        /// </summary>
        private readonly FileResult exampleResult;

        /// <summary>
        ///     The file result manager.
        /// </summary>
        private readonly IFileResultManager fileResultManager;

        /// <summary>
        ///     Initialises a new instance of the <see cref="FormatBuilder" /> class.
        /// </summary>
        /// <param name="currentFormat">
        ///     The current format.
        /// </param>
        /// <param name="fileResultManager">
        ///     The file result manager.
        /// </param>
        public FormatBuilder(string currentFormat, IFileResultManager fileResultManager)
        {
            InitializeComponent();
            this.fileResultManager = fileResultManager;
            exampleResult = FileResult.Example;
            textFormat.Text = currentFormat;
        }

        /// <summary>
        ///     Gets the format string.
        /// </summary>
        public string FormatString { get; private set; }

        /// <summary>
        ///     Handles the click event for the Cancel button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        ///     Handles the click event for the Date button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void DateButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{Date(" + dateFormat.Text + ")}";
        }

        /// <summary>
        ///     Handles the text of the date format changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void DateFormatTextChanged(object sender, EventArgs e)
        {
            try
            {
                dateExample.Text = DateTime.Today.ToString(dateFormat.Text);
            }
            catch
            {
                dateExample.Text = "Error";
            }
        }

        /// <summary>
        ///     Handles the click event for the Directory button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void DirectoryButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += Path.DirectorySeparatorChar;
        }

        /// <summary>
        ///     Handles the click event for the Episode Name button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void EpisodeNameButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{EName(" + GetSeparator() + ")}";
        }

        /// <summary>
        ///     Handles the click event Episode Number for the  button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void EpisodeNumberButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{ENum(" + digitalSelector.Value + ")}";
        }

        /// <summary>
        ///     Handles the click event for the File Extension button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void FileExtensionButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{Ext}";
        }

        /// <summary>
        ///     Handles the click event for the Folder Name button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void FolderNameButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{FName}";
        }

        /// <summary>
        ///     Handles the load event for the form.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void FormatBuilderLoad(object sender, EventArgs e)
        {
            wordSeparator.SelectedIndex = 0;
        }

        /// <summary>
        ///     Gets the word separator character.
        /// </summary>
        /// <returns>The word separator character.</returns>
        private string GetSeparator()
        {
            switch (wordSeparator.SelectedItem.ToString())
            {
                case "[space]":
                    return " ";
                default:
                    return wordSeparator.SelectedItem.ToString();
            }
        }

        /// <summary>
        ///     Handles the click event for the OK button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OkButtonClick(object sender, EventArgs e)
        {
            FormatString = textFormat.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        ///     Handles the click event for the Season Number button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void SeasonNumberButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{SNum(" + digitalSelector.Value + ")}";
        }

        /// <summary>
        ///     Handles the click event for the Show Name button.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ShowNameButtonClick(object sender, EventArgs e)
        {
            textFormat.Text += "{SName(" + GetSeparator() + ")}";
        }

        /// <summary>
        ///     Handles the text for changing.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void TextFormatTextChanged(object sender, EventArgs e)
        {
            try
            {
                textExample.Text = fileResultManager.FormatOutputPath(exampleResult, textFormat.Text);
            }
            catch
            {
                textExample.Text = "Error";
            }
        }
    }
}
