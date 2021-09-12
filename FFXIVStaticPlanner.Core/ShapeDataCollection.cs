using System.Collections.ObjectModel;

namespace FFXIVStaticPlanner.Data
{
    public class ShapeDataCollection : ObservableCollection<ShapeData>
    {
        bool _bInit;

        public bool Initializing => _bInit;

        public void BeginInit ( ) => _bInit = true;

        public void EndInit ( ) => _bInit = false;

    }
}
