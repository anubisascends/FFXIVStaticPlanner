using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FFXIVStaticPlanner.ViewModels
{
    /// <summary>
    /// When inherited into a child class, this provides the basic set of properties, methods and events to handle ViewModel functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ViewModel<T>: INotifyPropertyChanged where T : Window, new()
    {
        /// <summary>
        /// Gets the Window that this <see cref="ViewModel{T}"/> handles
        /// </summary>
        public T Window
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel{T}"/> class
        /// </summary>
        public ViewModel ( )
        {
            Window = new T
            {
                DataContext = this
            };
        }

        /// <summary>
        /// Raised whenver the value of a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Shows the view assigned to this view model
        /// </summary>
        public void Show ( ) => Window?.Show ( );

        /// <summary>
        /// Opens the view and only returns when the view is closed
        /// </summary>
        /// <returns>A <see cref="Nullable{T}"/> value of type <see cref="bool"/> that specifies whether the activity was accepted (<see langword="true"/>) or rejected (<see langword="false"/>).  The return value is <see cref="System.Windows.Window.DialogResult"/> property before a window closes.</returns>
        public bool? ShowDialog ( ) => Window?.ShowDialog ( );

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        public void RaisePropertyChanged ( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );
    }
}
