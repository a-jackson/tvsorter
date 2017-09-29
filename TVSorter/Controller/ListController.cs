// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ListController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for lists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Controller for lists.
    /// </summary>
    public class ListController : ControllerBase
    {
        /// <summary>
        ///     The list of items.
        /// </summary>
        private BindingList<string> itemList;

        /// <summary>
        ///     The title of the form.
        /// </summary>
        private string title;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ListController" /> class.
        /// </summary>
        /// <param name="list">
        ///     The list.
        /// </param>
        /// <param name="dialogTitle">
        ///     The dialog title.
        /// </param>
        public ListController(IList<string> list, string dialogTitle)
        {
            // Clone the list. That way, if the close button is used
            // the original list won't be affected.
            List = new BindingList<string>(list.ToList());
            Title = dialogTitle;
        }

        /// <summary>
        ///     Gets List.
        /// </summary>
        public BindingList<string> List
        {
            get => itemList;

            private set
            {
                itemList = value;
                OnPropertyChanged("List");
            }
        }

        /// <summary>
        ///     Gets Title.
        /// </summary>
        public string Title
        {
            get => title;

            private set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        ///     Adds an item to the list.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        public void Add(string item)
        {
            List.Add(item);
        }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            OnPropertyChanged("List");
            OnPropertyChanged("Title");
        }

        /// <summary>
        ///     Removes an item from the list.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        public void Remove(string item)
        {
            List.Remove(item);
        }
    }
}
