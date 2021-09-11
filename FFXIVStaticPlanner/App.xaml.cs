using FFXIVStaticPlanner.ViewModels;
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
            
            appView.Show ( );
        }
    }
}
