using System;

namespace UICommon.ViewModelBases
{
    public class TabViewModelBase<T> : ViewModelBase<T> where T : class
    {
        /// <summary>
        /// Whether this element should be able to be closed.
        /// </summary>
        public virtual bool Closable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// A callback to the tab manager to enable specifying this tab to be closed.
        /// </summary>
        public virtual Action<object> LoadSelectedTab
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor only setting the header value.
        /// </summary>
        public TabViewModelBase(string header)
            : base(header)
        {
        }

        /// <summary>
        /// Constructor setting the header and database value.
        /// </summary>
        public TabViewModelBase(string header, T database)
             : base(header, database)
        {
        }

        public TabViewModelBase(string header, Action<object> loadTab)
            : base(header)
        {
            LoadSelectedTab = loadTab;
        }

        /// <summary>
        /// Constructor setting the header and database value and the routine by which tabs are selected
        /// to be closed.
        /// </summary>
        public TabViewModelBase(string header, T database, Action<object> loadTab)
            : base(header, database)
        {
            LoadSelectedTab = loadTab;
        }

        /// <summary>
        /// Updates both the data and 
        /// </summary>
        public virtual void UpdateData(T dataToDisplay, Action<object> removeTab)
        {
            DataStore = null;
            DataStore = dataToDisplay;
        }
    }
}
