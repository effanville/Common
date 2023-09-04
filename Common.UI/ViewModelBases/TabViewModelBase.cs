using System;

namespace Common.UI.ViewModelBases
{
    /// <summary>
    /// View model base to be used for Tabs within a TabControl.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// A callback to the tab manager to load a tab from a specified key.
        /// </summary>
        public virtual Action<object> LoadSelectedTab
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor only setting the header value.
        /// </summary>
        protected TabViewModelBase(string header)
            : base(header)
        {
        }

        /// <summary>
        /// Constructor setting the header and database value.
        /// </summary>
        protected TabViewModelBase(string header, T database, UiGlobals uiGlobals)
             : base(header, database, uiGlobals)
        {
        }

        /// <summary>
        /// Constructor setting the header and database value and the routine by which tabs are selected
        /// to be closed.
        /// </summary>
        protected TabViewModelBase(string header, T database, Action<object> loadTab, UiGlobals uiGlobals)
            : base(header, database, uiGlobals)
        {
            LoadSelectedTab = loadTab;
        }

        /// <summary>
        /// Updates the data and provides a mechanism to remove the tab
        /// </summary>
        /// <param name="modelData">The data to update with.</param>
        /// <param name="removeTab"> Callback specifying whether this tab should be removed</param>
        public virtual void UpdateData(T modelData, Action<object> removeTab)
        {
            ModelData = null;
            ModelData = modelData;
        }
    }
}
