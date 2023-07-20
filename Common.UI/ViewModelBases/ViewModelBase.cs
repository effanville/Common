using System;

using Common.Structure.DataEdit;

namespace Common.UI.ViewModelBases
{
    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    public abstract class ViewModelBase<T> : PropertyChangedBase where T : class
    {
        /// <summary>
        /// The data to be used in this view model.
        /// </summary>
        public T DataStore
        {
            get;
            set;
        }

        /// <summary>
        /// Any string to use to display in a header or a title of a UI element.
        /// </summary>
        public virtual string Header
        {
            get;
        }

        /// <summary>
        /// Event for requesting an update of the underlying data.
        /// </summary>
        public EventHandler<UpdateRequestArgs<T>> UpdateRequest;

        /// <summary>
        /// handle the events raised in the above.
        /// </summary>
        protected void OnUpdateRequest(UpdateRequestArgs<T> e)
        {
            EventHandler<UpdateRequestArgs<T>> handler = UpdateRequest;
            if (handler != null)
            {
                handler?.Invoke(null, e);
            }
        }

        /// <summary>
        /// Constructor only setting the header value.
        /// </summary>
        public ViewModelBase(string header)
        {
            Header = header;
        }

        /// <summary>
        /// Constructor setting the header and database values.
        /// </summary>
        public ViewModelBase(string header, T database)
        {
            Header = header;
            DataStore = database;
        }

        /// <summary>
        /// Mechanism to update the data 
        /// </summary>
        public virtual void UpdateData(T dataToDisplay)
        {
            DataStore = null;
            DataStore = dataToDisplay;
        }
    }
}
