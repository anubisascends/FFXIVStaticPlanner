using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FFXIVStaticPlanner.ViewModels
{
    public abstract class ViewModel<T>: INotifyPropertyChanged where T : Window, new()
    {
        public T Window
        {
            get;
            private set;
        }

        public ViewModel ( )
        {
            Window = new T
            {
                DataContext = this
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void Show ( ) => Window?.Show ( );

        public bool? ShowDialog ( ) => Window?.ShowDialog ( );

        public void RaisePropertyChanged ( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );
    }
}
