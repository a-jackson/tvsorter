// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ListController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for lists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for lists.
    /// </summary>
    public class ListController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   The list of items.
        /// </summary>
        private BindingList<string> itemList;

        /// <summary>
        ///   The title of the form.
        /// </summary>
        private string title;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListController"/> class.
        /// </summary>
        /// <param name="list">
        /// The list. 
        /// </param>
        /// <param name="dialogTitle">
        /// The dialog title. 
        /// </param>
        public ListController(IList<string> list, string dialogTitle)
        {
            // Clone the list. That way, if the close button is used
            // the original list won't be affected.
            this.List = new BindingList<string>(list.ToList());
            this.Title = dialogTitle;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets List.
        /// </summary>
        public BindingList<string> List
        {
            get
            {
                return this.itemList;
            }

            private set
            {
                this.itemList = value;
                this.OnPropertyChanged("List");
            }
        }

        /// <summary>
        ///   Gets Title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            private set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">
        /// The item. 
        /// </param>
        public void Add(string item)
        {
            this.List.Add(item);
        }

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.OnPropertyChanged("List");
            this.OnPropertyChanged("Title");
        }

        /// <summary>
        /// Removes an item from the list.
        /// </summary>
        /// <param name="item">
        /// The item. 
        /// </param>
        public void Remove(string item)
        {
            this.List.Remove(item);
        }

        #endregion
    }
}