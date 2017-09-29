// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SettingsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the settings tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TVSorter.Storage;
using TVSorter.View;
using Settings = TVSorter.Model.Settings;

namespace TVSorter.Controller
{
    /// <summary>
    ///     The controller for the settings tab.
    /// </summary>
    public class SettingsController : ControllerBase
    {
        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     The current settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///     Initialises a new instance of the <see cref="SettingsController" /> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        public SettingsController(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        ///     Gets the settings.
        /// </summary>
        public Settings Settings
        {
            get => settings;

            private set
            {
                settings = value;
                OnPropertyChanged("Settings");
            }
        }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            Revert();
        }

        /// <summary>
        ///     Reverts the settings.
        /// </summary>
        public void Revert()
        {
            Settings = storageProvider.LoadSettings();
        }

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        public void Save()
        {
            storageProvider.SaveSettings();
        }
    }
}
