using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.UI.ViewModelBases
{
    /// <summary>
    /// Base implementation of INotifyPropertyChanged interface
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler controlling property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// invokes the event for property changed.
        /// </summary>
        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the existing value if different from the new value
        /// and raises OnPropertyChanged if so.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="newValue">The new value to update with.</param>
        public void SetAndNotify<T>(ref T existingValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(existingValue, newValue))
            {
                existingValue = newValue;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
