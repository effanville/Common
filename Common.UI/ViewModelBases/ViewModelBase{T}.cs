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
        /// Constructor setting the header and database values.
        /// </summary>
        protected ViewModelBase(string header, T modelData) : base(header, modelData)
        {
        }
    }
}
