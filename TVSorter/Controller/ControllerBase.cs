// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ControllerBase.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Base type for controllers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Base type for controllers.
    /// </summary>
    public abstract class ControllerBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public abstract void Initialise(IView view);

        /// <summary>
        ///     Raises a property changed event.
        /// </summary>
        /// <param name="name">
        ///     The name of the property being changed.
        /// </param>
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
