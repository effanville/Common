using System;

using Effanville.Common.Structure.Reporting;

namespace Effanville.Common.UI.ViewModelBases;

/// <summary>
/// A view model base for storing base model data and an updater for a
/// global type
/// </summary>
/// <typeparam name="TModel">The type of the model data to display.</typeparam>
public abstract class ViewModelBase<TModel> : PropertyChangedBase
    where TModel : class
{
    private string _header;

    /// <summary>
    /// The globals for this view model.
    /// </summary>
    protected readonly UiGlobals DisplayGlobals;

    /// <summary>
    /// The logging mechanism.
    /// </summary>
    public IReportLogger ReportLogger => DisplayGlobals.ReportLogger;

    public EventHandler ModelUpdated;

    /// <summary>
    /// The data for the model in this view model.
    /// </summary>
    public TModel ModelData
    {
        get;
        protected set;
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
    /// Generate a <see cref="ViewModelBase{TModel}"/> with a
    /// specific header.
    /// </summary>
    protected ViewModelBase(string header, UiGlobals globals)
    {
        Header = header;
        DisplayGlobals = globals;
    }

    /// <summary>
    /// Generate a <see cref="ViewModelBase{TModel}"/> with a
    /// specific header and a specified model.
    /// </summary>
    protected ViewModelBase(string header, TModel modelData, UiGlobals displayGlobals)
    {
        Header = header;
        ModelData = modelData;
        DisplayGlobals = displayGlobals;
    }

    /// <summary>
    /// Mechanism to update the data 
    /// </summary>
    public virtual void UpdateData(TModel modelData, bool force)
    {
        ModelData = null;
        ModelData = modelData;
    }

    /// <summary>
    /// Use to raise a model data change event
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnModelUpdated(EventArgs e)
        => ModelUpdated?.Invoke(this, e);
}