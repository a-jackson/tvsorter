// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ListDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog that shows a list of strings and allows adding and removing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows.Forms;
using TVSorter.Controller;

namespace TVSorter.View
{
    /// <summary>
    ///     A dialog that shows a list of strings and allows adding and removing.
    /// </summary>
    public partial class ListDialog : Form, IView
    {
        /// <summary>
        ///     The controller.
        /// </summary>
        private readonly ListController controller;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ListDialog" /> class.
        /// </summary>
        /// <param name="listController">
        ///     The list controller.
        /// </param>
        public ListDialog(ListController listController)
        {
            InitializeComponent();
            controller = listController;
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
            // Not needed.
        }

        /// <summary>
        ///     Handles Add button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void AddButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(text.Text))
            {
                controller.Add(text.Text);
            }

            text.ResetText();
            text.Focus();
        }

        /// <summary>
        ///     Handles the close button being clicked.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        ///     Handles the List dialog loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void ListDialogLoad(object sender, EventArgs e)
        {
            controller.Initialise(this);
        }

        /// <summary>
        ///     Handles the event for a property changing in the controller.
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
                case "List":
                    list.DataSource = controller.List;
                    break;
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
        private void RemoveButtonClick(object sender, EventArgs e)
        {
            if (list.SelectedIndex != -1)
            {
                controller.Remove((string)list.SelectedItem);
            }
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
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
