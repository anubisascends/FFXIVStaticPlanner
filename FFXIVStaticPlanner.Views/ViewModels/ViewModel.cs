using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FFXIVStaticPlanner.ViewModels
{
    public abstract class ViewModel: INotifyPropertyChanged
    {
        public Window Window
        {
            get;
            private set;
        }

        public ViewModel ( ) => Initialize ( );

        public event PropertyChangedEventHandler PropertyChanged;
        public void Initialize ( )
        {
            var type = GetType();
            var viewName = type.FullName.Replace("Model", "").Replace("ViewModels", "Views");

            if ( Type.GetType(viewName) is Type viewType )
            {
                Window = Activator.CreateInstance ( viewType ) as Window;
                Window.DataContext = this;
            }
        }

        public void Show ( ) => Window?.Show ( );

        public bool? ShowDialog ( ) => Window?.ShowDialog ( );

        public void RaisePropertyChanged ( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );
    }
}
