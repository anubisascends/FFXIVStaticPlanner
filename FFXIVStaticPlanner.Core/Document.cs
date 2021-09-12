using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Ink;
using System.Windows.Shapes;

namespace FFXIVStaticPlanner.Data
{

    public class Document : INotifyPropertyChanged
    {
        private string strFileName;
        private StrokeCollection _objStrokes = new();
        private ObservableCollection<ImageIcon> _objImages;
        private bool _bChanged;
        private ObservableCollection<ShapeData> _objShapes;

        public Document ( )
        {
            _objStrokes = new ( );
            _objImages = new ( );
            _objShapes = new ( );

            _objStrokes.StrokesChanged += onStrokesChanged;
            _objImages.CollectionChanged += onImagesChanged;
            _objShapes.CollectionChanged += onShapesChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public string FileName
        {
            get => strFileName;
            set
            {
                strFileName = value;
                raisePropertyChanged ( );
            }
        }

        public StrokeCollection Strokes => _objStrokes;

        public ObservableCollection<ImageIcon> Images => _objImages;

        public ObservableCollection<ShapeData> Shapes => _objShapes;

        public bool HasChanges
        {
            get => _bChanged;
            set
            {
                _bChanged = value;
                raisePropertyChanged ( );
            }
        }

        private void raisePropertyChanged ( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );

        private void onImagesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;

        private void onStrokesChanged ( object sender , StrokeCollectionChangedEventArgs e ) => HasChanges = true;

        private void onShapesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;
    }
}
