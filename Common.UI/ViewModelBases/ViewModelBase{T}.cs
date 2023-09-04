using System;

namespace Common.UI.ViewModelBases
    {

    /// <summary>
    /// Base for ViewModels containing display purpose objects.
    /// </summary>
    public abstract class ViewModelBase<T> : ViewModelBase<T,T> where T : class
    {
        /// <summary>
        /// Constructor only setting the header value.
        /// </summary>
        protected ViewModelBase(string header) 
            : base(header)
        {
        }
        
        /// <summary>
        /// Generate a <see cref="ViewModelBase{TModel}"/> with a
        /// specific header.
        /// </summary>
        protected ViewModelBase(string header, UiGlobals globals)
            : base(header, globals)
        {
        }
        
        /// <summary>
        /// Constructor setting the header and database values.
        /// </summary>
        protected ViewModelBase(string header, T modelData) 
            : base(header, modelData)
        {
        }    
        
        /// <summary>
        /// Generate a <see cref="ViewModelBase{TModel}"/> with a
        /// specific header and a specified model.
        /// </summary>
        protected ViewModelBase(string header, T modelData, UiGlobals displayGlobals)
            : base(header, modelData, displayGlobals)
        {
        }
    }
}
