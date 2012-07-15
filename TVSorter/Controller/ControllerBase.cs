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

    using System.ComponentModel;

    using TVSorter.View;

    #endregion

    /// <summary>
    /// Base type for controllers.
    /// </summary>
    public abstract class ControllerBase : INotifyPropertyChanged
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

        #endregion
    }
}