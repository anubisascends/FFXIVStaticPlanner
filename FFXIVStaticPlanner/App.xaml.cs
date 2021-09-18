using FFXIVStaticPlanner.ViewModels;
using System.Reflection;
using System.Windows;

namespace FFXIVStaticPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup ( StartupEventArgs e )
        {
            var appView = new RootViewModel();

            appView.SetVersion ( Assembly.GetAssembly ( typeof ( App ) ).GetName ( ).Version.ToString ( ) );
            appView.Show ( );
        }
    }
}
