using System;
using System.Windows.Input;

using Common.UI.Commands;

namespace Common.UI.ViewModelBases;

/// <summary>
/// A view model base that enables closing, and invokes a closing action when closed.
/// </summary>
/// <typeparam name="T">The type for the ModelData</typeparam>
public abstract class ClosableViewModelBase<T> : ViewModelBase<T> where T : class
{
    private bool _closable;

    /// <summary>
    /// An event where this view model requests to close.
    /// </summary>
    public EventHandler RequestClose;
        
    /// <summary>
    /// Whether the display can be closed or not.
    /// </summary>
    public bool Closable
    {
        get => _closable;
        set => SetAndNotify(ref _closable, value);
    }
        
    /// <summary>
    /// handle the events raised in the above.
    /// </summary>
    protected void OnRequestClose(EventArgs e)
    {
        if (!Closable)
        {
            return;
        }

        EventHandler handler = RequestClose;
        handler?.Invoke(this, e);
    }

    /// <summary>
    /// Command for initiating the close.
    /// </summary>
    public ICommand CloseCommand
    {
        get;
        set;
    }

    private void InitiateClose() => OnRequestClose(EventArgs.Empty);

    protected ClosableViewModelBase(string header, UiGlobals globals, bool closable) 
        : base(header, globals)
    {
        Closable = closable;
        CloseCommand = new RelayCommand(InitiateClose);
    }

    protected ClosableViewModelBase(string header, T modelData, UiGlobals displayGlobals, bool closable)
        : base(header, modelData, displayGlobals)
    {
        Closable = closable;
        CloseCommand = new RelayCommand(InitiateClose);
    }
}