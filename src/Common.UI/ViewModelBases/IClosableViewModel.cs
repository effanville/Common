using System;
using System.Windows.Input;

namespace Effanville.Common.UI.ViewModelBases;

/// <summary>
/// A view model that defines and enables closing of the view
/// </summary>
public interface IClosableViewModel
{
    /// <summary>
    /// An event where this view model requests to close.
    /// </summary>
    EventHandler RequestClose { get; set; }

    /// <summary>
    /// Whether the display can be closed or not.
    /// </summary>
    bool Closable { get; set; }

    /// <summary>
    /// Command for initiating the close.
    /// </summary>
    ICommand CloseCommand { get; set; }
}
