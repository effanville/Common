using System;

using Common.Structure.DataEdit;

namespace Common.UI.ViewModelBases;

/// <summary>
/// A view model base for storing base model data and an updater for a
/// global type
/// </summary>
/// <typeparam name="TModel">The type of the model data to display.</typeparam>
/// <typeparam name="TUpdate">The type of the model one wishes to update.</typeparam>
public abstract class ViewModelBase<TModel, TUpdate> : PropertyChangedBase
    where TModel : class where TUpdate : class
{
    private string _header;
    /// <summary>
    /// The data for the model in this view model.
    /// </summary>
    public TModel ModelData
    {
        get;
        set;
    }
        
    /// <summary>
    /// Any string to use to display in a header or a title of a UI element.
    /// </summary>
    public string Header
    {
        get => _header;
        set => SetAndNotify(ref _header, value);
    }
        
    /// <summary>
    /// Event for requesting an update of the underlying data.
    /// </summary>
    public EventHandler<UpdateRequestArgs<TUpdate>> UpdateRequest;

    /// <summary>
    /// Generate a <see cref="ViewModelBase{TModel, TUpdate}"/> with a
    /// specific header.
    /// </summary>
    protected ViewModelBase(string header)
    {
        _header = header;
    }
        
    /// <summary>
    /// Generate a <see cref="ViewModelBase{TModel, TUpdate}"/> with a
    /// specific header and a specified model.
    /// </summary>
    protected ViewModelBase(string header, TModel modelData)
    {
        Header = header;
        ModelData = modelData;
    }

    /// <summary>
    /// handle the events raised in the above.
    /// </summary>
    protected void OnUpdateRequest(UpdateRequestArgs<TUpdate> e)
    {
        EventHandler<UpdateRequestArgs<TUpdate>> handler = UpdateRequest;
        if (handler != null)
        {
            handler?.Invoke(null, e);
        }
    }
        
    /// <summary>
    /// Mechanism to update the data 
    /// </summary>
    public virtual void UpdateData(TModel dataToDisplay)
    {
        ModelData = null;
        ModelData = dataToDisplay;
    }
}