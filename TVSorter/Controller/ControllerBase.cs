// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ControllerBase.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Base type for controllers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Controller
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    using TVSorter.View;

    #endregion

    /// <summary>
    /// Base type for controllers.
    /// </summary>
    public abstract class ControllerBase : INotifyPropertyChanged, IProgressTask
    {
        #region Public Events

        /// <summary>
        ///   Occurs when the progress changes.
        /// </summary>
        public event EventHandler ProgressChanged;

        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///   Occurs when the task is complete.
        /// </summary>
        public event EventHandler TaskComplete;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the max value of the progress.
        /// </summary>
        public int MaxValue { get; protected set; }

        /// <summary>
        ///   Gets or sets the current value of the progress.
        /// </summary>
        public int Value { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public abstract void Initialise(IView view);

        #endregion

        #region Methods

        /// <summary>
        /// Raises a progress changed event.
        /// </summary>
        protected void OnProgressChanged()
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises a property changed event.
        /// </summary>
        /// <param name="name">
        /// The name of the property being changed. 
        /// </param>
        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Raise a task complete event.
        /// </summary>
        protected void OnTaskComplete()
        {
            if (this.TaskComplete != null)
            {
                this.TaskComplete(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}