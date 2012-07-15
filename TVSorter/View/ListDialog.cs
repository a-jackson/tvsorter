// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ListDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog that shows a list of strings and allows adding and removing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using TVSorter.Controller;

    #endregion

    /// <summary>
    /// A dialog that shows a list of strings and allows adding and removing.
    /// </summary>
    public partial class ListDialog : Form, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private readonly ListController controller;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDialog"/> class.
        /// </summary>
        /// <param name="listController">
        /// The list controller. 
        /// </param>
        public ListDialog(ListController listController)
        {
            this.InitializeComponent();
            this.controller = listController;
            this.controller.PropertyChanged += this.PropertyChanged;
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
            // Not needed.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles Add button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void AddButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.text.Text))
            {
                this.controller.Add(this.text.Text);
            }

            this.text.ResetText();
            this.text.Focus();
        }

        /// <summary>
        /// Handles the close button being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void CloseButtonClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handles the List dialog loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ListDialogLoad(object sender, EventArgs e)
        {
            this.controller.Initialise(this);
        }

        /// <summary>
        /// Handles the event for a property changing in the controller.
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
                case "Title":
                    this.Text = this.controller.Title;
                    break;
                case "List":
                    this.list.DataSource = this.controller.List;
                    break;
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
        private void RemoveButtonClick(object sender, EventArgs e)
        {
            if (this.list.SelectedIndex != -1)
            {
                this.controller.Remove((string)this.list.SelectedItem);
            }
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}